namespace JobApplicationSpam
open FSharp.Data.Sql
module DBTypes =
    [<Literal>]
    let private connectionString = "Server=localhost; Port=5432; User Id=spam; Password=Steinmetzstr9!@#$; Database=jobapplicationspam"
    [<Literal>]
    let private resolutionPath = "bin"
    type DB =
        SqlDataProvider<
            DatabaseVendor = FSharp.Data.Sql.Common.DatabaseProviderTypes.POSTGRESQL,
            ConnectionString = connectionString,
            ResolutionPath = resolutionPath,
            IndividualsAmount = 1000,
            UseOptionTypes = true>
    let dbContext = DB.GetDataContext()
    let db = dbContext.Public

