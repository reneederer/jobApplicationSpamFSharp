namespace JobApplicationSpam
open FSharp.Data.Sql
open FSharp.Data.Sql.Transactions
module DBTypes =
    open System
    open log4net
    open System.Reflection
    open Chessie.ErrorHandling
    open System.Transactions
    open WebSharper.Core.JavaScript.Syntax
    open WebSharper.UI.Next.Html
    open FSharpPlus.Operators

    let log = LogManager.GetLogger(MethodBase.GetCurrentMethod().GetType())

    [<Literal>]
    let private connectionString = "Server=localhost; Port=5432; User Id=spam; Password=Steinmetzstr9!@#$; Database=jobapplicationspam; Enlist=true"
    [<Literal>]
    let private resolutionPath = "bin"
    type DB =
        SqlDataProvider<
            DatabaseVendor = FSharp.Data.Sql.Common.DatabaseProviderTypes.POSTGRESQL,
            ConnectionString = connectionString,
            ResolutionPath = resolutionPath,
            IndividualsAmount = 1000,
            UseOptionTypes = true>
    [<Literal>]
    let private connectionStringTest = "Server=localhost; Port=5432; User Id=spam; Password=Steinmetzstr9!@#$; Database=jobapplicationspamtest"
    let private defaultContext = DB.GetDataContext()

    let readDB f =
        try
            f defaultContext
        with
        | e -> 
            log.Error e
            failwith "Couldn't read from database"
     
    let andThen g f (dbContext : DB.dataContext) =
        let r = f dbContext
        dbContext.SubmitUpdates()
        g r dbContext

    let withTransaction (f : DB.dataContext -> 'a) =
        try
            use dbScope = new TransactionScope()
            let dbContext = DB.GetDataContext()
            let r = f dbContext
            dbContext.SubmitUpdates()
            dbScope.Complete()
            r
        with
        | e -> 
            log.Error e
            failwith "Transaction failed!"
    