;set client_encoding to 'UTF8';

drop table if exists link cascade;
drop table if exists sentStatus cascade;
drop table if exists sentStatusValue cascade;
drop table if exists sentApplication cascade;
drop table if exists sentFilePage cascade;
drop table if exists sentDocument cascade;
drop table if exists sentUserValues cascade;
drop table if exists sentDocumentEmail cascade;
drop table if exists pageMap cascade;
drop table if exists lastEditedDocumentId cascade;
drop table if exists htmlPage cascade;
drop table if exists filePage cascade;
drop table if exists documentEmail cascade;
drop table if exists document cascade;
drop table if exists htmlPageTemplate cascade;
drop table if exists jobRequirement cascade;
drop table if exists jobOffer cascade;
drop table if exists employer cascade;
drop table if exists userValues cascade;
drop table if exists login cascade;
drop table if exists users cascade;


create table users (id serial primary key, email text unique null, password text not null, salt text not null, confirmEmailGuid text null, sessionGuid text unique null, createdOn date not null);
create table login (id serial primary key, userId int not null, loggedInAt timestamp with time zone, foreign key(userId) references users(id));
create table userValues(id serial primary key, userId int unique not null, gender text not null, degree text not null, firstName text not null, lastName text not null, street text not null, postcode text not null, city text not null,phone text not null, mobilePhone text not null, foreign key(userId) references users(id));
create table employer(id serial primary key, userId int not null, company text not null, street text not null, postcode text not null, city text not null, gender text not null, degree text not null, firstName text not null, lastName text not null, email text not null, phone text not null, mobilePhone text not null, foreign key(userId) references users(id));
create table jobOffer(id serial primary key, url text not null, jobName text not null);
create table jobRequirement(id serial primary key, jobOfferId int not null, key text not null, value text not null, foreign key(jobOfferId) references jobOffer(id));
create table htmlPageTemplate(id serial primary key, name text unique not null, odtPath text unique not null, html text not null);
create table document(id serial primary key, userId int not null, name text not null, jobName text not null, customVariables text not null, foreign key(userId) references users(id));
create table lastEditedDocumentId(userId int unique primary key not null, documentId int not null, foreign key(userId) references users(id), foreign key(documentId) references document(id));
create table filePage(id serial primary key, documentId int not null, path text not null, pageIndex int not null, name text not null, foreign key(documentId) references document(id), constraint filePage_unique unique(documentId, pageIndex));
create table htmlPage(id serial primary key, documentId int not null, templateId int null, pageIndex int not null, name text not null, foreign key(documentId) references document(id), foreign key(templateId) references htmlPageTemplate(id), constraint htmlPage_unique unique(documentId, pageIndex));
create table documentEmail(id serial primary key, documentId int not null unique, subject text not null, body text not null, foreign key(documentId) references document(id));
create table pageMap(id serial primary key, documentId int not null, pageIndex int not null, key text not null, value text not null, foreign key(documentId) references document(id), constraint pageMap_unique unique(documentId, pageIndex, key));

create table sentDocumentEmail(id serial primary key, subject text not null, body text not null);
create table sentUserValues(id serial primary key, email text not null, gender text not null, degree text not null, firstName text not null, lastName text not null, street text not null, postcode text not null, city text not null, phone text not null, mobilePhone text not null);
create table sentDocument(id serial primary key, employerId int not null, sentDocumentEmailId int not null, sentUserValuesId int not null, jobName text not null, customVariables text not null, foreign key(sentUserValuesId) references sentUserValues(id) on update cascade, foreign key(employerId) references employer(id), foreign key(sentDocumentEmailId) references sentDocumentEmail(id) on update cascade);
create table sentFilePage(id serial primary key, sentDocumentId int not null, path text not null, pageIndex int not null, foreign key(sentDocumentId) references sentDocument(id) on update cascade);

create table sentApplication(id serial primary key, userId int not null, sentDocumentId int not null, url text not null, foreign key(userId) references users(id), foreign key(sentDocumentId) references sentDocument(id) on update cascade);
create table sentStatusValue(id int primary key, status text not null);
create table sentStatus(id serial primary key, sentApplicationId int not null, statusChangedOn date not null, dueOn timestamp null, sentStatusValueId int not null, statusMessage text not null, foreign key(sentApplicationId) references sentApplication(id), foreign key(sentStatusValueId) references sentStatusValue(id));
create table link(id serial primary key, path text not null, guid text not null, name text not null);

insert into users(email, password, salt, confirmEmailGuid, sessionGuid, createdOn) values('ene.ederer.nbg@gmail.com', 'r99n/4/4NGGeD7pn4I1STI2rI+BFweUmzAqkxwLUzFP9aB7g4zR5CBHx+Nz2yn3NbiY7/plf4ZRGPaXXnQvFsA==', 'JjjYQTWgutm4pv/VnzgHf6r4NjNrAVcTq+xnR7/JsRGAIHRdrcw3IMVrzngn2KPRakfX/S1kl9VrqwAT+T02Og==', null, '1234', current_date);
/*
insert into users(email, password, salt, guid) values('ren.ederer.nbg@gmail.com', 'r99n/4/4NGGeD7pn4I1STI2rI+BFweUmzAqkxwLUzFP9aB7g4zR5CBHx+Nz2yn3NbiY7/plf4ZRGPaXXnQvFsA==', 'JjjYQTWgutm4pv/VnzgHf6r4NjNrAVcTq+xnR7/JsRGAIHRdrcw3IMVrzngn2KPRakfX/S1kl9VrqwAT+T02Og==', null);
insert into users(email, password, salt, guid) values('helmut.goerke@gmail.com', 'r99n/4/4NGGeD7pn4I1STI2rI+BFweUmzAqkxwLUzFP9aB7g4zR5CBHx+Nz2yn3NbiY7/plf4ZRGPaXXnQvFsA==', 'JjjYQTWgutm4pv/VnzgHf6r4NjNrAVcTq+xnR7/JsRGAIHRdrcw3IMVrzngn2KPRakfX/S1kl9VrqwAT+T02Og==', 'someguid');
insert into users(email, password, salt, guid) values('r', 'r99n/4/4NGGeD7pn4I1STI2rI+BFweUmzAqkxwLUzFP9aB7g4zR5CBHx+Nz2yn3NbiY7/plf4ZRGPaXXnQvFsA==', 'JjjYQTWgutm4pv/VnzgHf6r4NjNrAVcTq+xnR7/JsRGAIHRdrcw3IMVrzngn2KPRakfX/S1kl9VrqwAT+T02Og==', null);

*/
insert into userValues(userId, gender, degree, firstName, lastName, street, postcode, city, phone, mobilePhone) values(1, 'm', '', 'René', 'Ederer', 'Raabstr. 24A', '90429', 'Nürnberg', 'kein Telefon', 'kein Handy');
/*
insert into userValues(userId, gender, degree, firstName, lastName, street, postcode, city, phone, mobilePhone) values(2, 'm', '', 'Helmut', 'Görke', 'Raabstr. 24A', '90429', 'Nürnberg', '0911 918273', '01520 2723494');
*/
/*
insert into employer(userId, company, street, postcode, city, gender, degree, firstName, lastName, email, phone, mobilePhone) values(1, 'BJC BEST JOB IT SERVICES GmbH', 'Alte Rabenstraße 32', '20148', 'Hamburg', 'f', '', 'Katrin', 'Thoms', 'Katrin.Thoms@bjc-its.de', '+49 (40) 5 14 00 7180', '');
insert into employer(userId, company, street, postcode, city, gender, degree, firstName, lastName, email, phone, mobilePhone) values(1, 'Deutsche Anwaltshotline AG', 'Am Plärrer 7', '90443', 'Nürnberg', 'm', '', 'Jonas', 'Zimmermann', 'mail@deutsche-anwaltshotline.de', '+49 911 3765690', '');
insert into employer(userId, company, street, postcode, city, gender, degree, firstName, lastName, email, phone, mobilePhone) values(1, 'ANG.-Punkt und gut! GmbH', 'Südwestpark 37-41', '90449', 'Nürnberg', 'f', '', 'Jaqueline', 'Strauß', 'bewerbung@ang.de', '+49 911 525700', '+49 1778876348');
insert into employer(userId, company, street, postcode, city, gender, degree, firstName, lastName, email, phone, mobilePhone) values(1, 'iQ-Bewerbermanagement', 'Obernstr. 111', '28832', 'Achim bei Bremen', 'f', '', 'Nele', 'Sommerfeld', 'nele.sommerfeld@iq-bewerbermanagement.de', '+49 40 6003852232', '');
insert into employer(userId, company, street, postcode, city, gender, degree, firstName, lastName, email, phone, mobilePhone) values(1, 'engineering people GmbH', 'Südwestpark 60', '90449', 'Nürnberg',  'm', '', 'Haluk', 'Acar','haluk.acar@engineering-people.de', '+49 911 239560316', '');
insert into employer(userId, company, gender, street, postcode, city, degree, firstName, lastName, email, phone, mobilePhone) values(1, 'BFI Informationssysteme GmbH', 'Ötterichweg 7', '90411', 'Nürnberg', 'm', '', 'Michael', 'Schlund', 'Michael.Schlund@bfi-info.de', '0911 9457668', '');
*/
insert into htmlPageTemplate(name, odtPath, html) values('Anschreiben nach DIN 5008', 'c:/users/rene/desktop/bewerbung_neu.odt',
'<div id="divInsert">
<div id="divTemplate" class="page1">
    <div style="width: 100%; background-color: white">
        <input type=text data-bind-ref="employerGender" data-bind-value="m" />
        <input type=text data-bind-ref="employerGender" data-bind-value="u" />
        <input type=text data-bind-ref="employerGender" data-bind-value="f" />
        <input class="resizing" autofocus "autofocus" style="font-family: Arial; font-size: 12pt; font-weight: normal" data-bind-ref="userDegree" data-variable-value="userDegree" placeholder="Dein Titel" />
        <input class="resizing" value="test" style="border:none; outline: none; font-family: Arial; font-size: 12pt; font-weight: normal"  data-variable-value="userFirstName" data-bind-ref="userFirstName" placeholder="Dein Vorname" />
        <input class="resizing" style="border:none; outline: none; letter-spacing: 0px; font-family: Arial; font-size: 12pt; font-weight: normal"  data-bind-ref="userLastName" data-variable-value="userLasssstName" placeholder="Dein Nachname" />
        <br />
        <input class="resizing" style="border:none; outline: none; font-family: Arial; font-size: 12pt; font-weight: normal"  data-bind-ref="userStreet" style= "width:150px" data-variable-value="userStreet" placeholder="Deine Straße" />
        <br />
        <input class="resizing" style="border:none; outline: none; font-family: Arial; font-size: 12pt; font-weight: normal"  data-bind-ref="userPostcode" data-variable-value="userPostcode" placeholder="Deine Postleitzahl" />
        <input class="resizing" style="border:none; outline: none; font-family: Arial; font-size: 12pt; font-weight: normal"  data-bind-ref="userCity" data-variable-value="userCity" placeholder="Deine Stadt" />
        <br />
        <br />
        <br />
        <br />
        <input class="resizing" style="border:none; outline: none; font-family: Arial; font-size: 12pt; font-weight: normal"  data-bind-ref="employerDegree" placeholder="Chef-Titel" />
        <input class="resizing" style="border:none; outline: none; font-family: Arial; font-size: 12pt; font-weight: normal"  data-bind-ref="employerFirstName" placeholder="Chef-Vorname" />
        <input class="resizing" style="border:none; outline: none; font-family: Arial; font-size: 12pt; font-weight: normal"  data-bind-ref="employerLastName" placeholder="Chef-Nachname" />
        <br />
        <input class="resizing" style="border:none; outline: none; font-family: Arial; font-size: 12pt; font-weight: normal"  data-bind-ref="employerStreet" placeholder="Firma-Strasse" />
        <br />
        <input class="resizing" style="border:none; outline: none; font-family: Arial; font-size: 12pt; font-weight: normal"  data-bind-ref="employerPostcode" placeholder="Firma-Postleitzahl" />
        <input class="resizing" style="border:none; outline: none; font-family: Arial; font-size: 12pt; font-weight: normal"  data-bind-ref="employerCity" placeholder="Firma-Stadt" />
        <br />
<span style= "float:right">
<input type="text" readonly="readonly" class="resizing" style="font-family: Arial; font-size: 12pt; font-weight: normal; border: none; outline: none;padding:0px; margin:0px" data-bind-ref="userCity" />,&nbsp
<input type="text" readonly="readonly" class="resizing" style="font-family: Arial; font-size: 12pt; font-weight: normal" data-variable-value="today" />
</span>
        <br />
        <br />
        <input class="resizing" data-bind-ref="documentEmailSubject" style="border:none; outline: none; font-family: Arial; font-size: 12pt; font-weight: bold" placeholder="Betreff" />
        <br />
        <br />
    </div>
    <div style="width: 100%;">
        <textarea rows="7" data-page-key="mainText" data-page-value="ehr geehrte Damen und Herren" style="wrap: soft; border: solid 2px red; outline: none; letter-spacing:0pt; margin: 0px; padding: 0px; overflow: hidden; min-height: 100%; min-width: 100%; font-family: Arial; font-size: 12pt; font-weight: normal; display: block"></textarea>
        <textarea rows="7" data-page-key="mainText1" data-page-value="Sehr geehrte Damen und Herren" style="wrap: soft; border: solid 2px red; outline: none; letter-spacing:0pt; margin: 0px; padding: 0px; overflow: hidden; min-height: 100%; min-width: 100%; font-family: Arial; font-size: 12pt; font-weight: normal; display: block"></textarea>
    </div>
    <div style="width: 100%">
        <br />
        Mit freundlichen Grüßen
        <br />
        <br />
        <br />
<input type="text" readonly="readonly" class="resizing" style="border: none; outline: none;font-family: Arial; font-size: 12pt; font-weight: normal" data-bind-ref="userDegree" />&nbsp;
<input type="text" readonly="readonly" class="resizing" style="border: none; outline: none;font-family: Arial; font-size: 12pt; font-weight: normal" data-bind-ref="userFirstName" />&nbsp;
<input type="text" readonly="readonly" class="resizing" style="border: none; outline: none;font-family: Arial; font-size: 12pt; font-weight: normal" data-bind-ref="userLastName" />
</div>
</div>
</div>');
insert into htmlPageTemplate(name, odtPath, html) values('Deckblatt', 'c:/users/rene/desktop/bewerbung_deckblatt.odt',
'<div id="insertDiv">
<h1>Deckblatt</h1>
<input class="resizing" style="border:none; outline: none; font-family: Arial; font-size: 12pt; font-weight: normal" data-bind-ref="employerFirstName" placeholder="Chef-Vorname" />
<image src="null" width="400" height="100" />
<input type="text" class="resizing" data-bind-ref="userFirstName"></input>
<input type="text" data-page-mainText="xainText"></input>
<br />
<input type="text" class="resizing" style="border: none; outline: none;font-family: Arial; font-size: 12pt; font-weight: normal" data-bind-ref="userLastName" placeholder="Dein Name" />
hallo div!</div>
');
insert into htmlPageTemplate(name, odtPath, html) values('Lebenslauf', 'c:/users/rene/desktop/bewerbung_lebenslauf.odt', '<div id="insertDiv"><b>Lebenslauf...</b></div>');
insert into document(userId, name, jobName, customVariables) values(1, 'mein htmlTemplate', 'Fachinformatiker', '');
insert into document(userId, name, jobName, customVariables) values(1, 'mein zweites htmlTemplate', 'Automechaniker', '');
insert into documentEmail(documentId, subject, body) values(1, 'Bewerbung als $beruf', 'Sehr $geehrter $chefAnrede $chefTitel $chefNachname,\n\nanbei sende ich Ihnen meine Bewerbungsunterlagen.\nÜber eine Einladung zu einem Bewerbungsgespräch freue ich mich sehr.\n\nMit freundlichen Grüßen\n\n\n$meinTitel $meinVorname $meinNachname\n$meineStrasse\n$meinePlz $meineStadt\n$meineMobilnummer');
insert into filePage(documentId, path, pageIndex, name) values(1, 'Users/1/bewerbung_neu.odt', 1, 'Anschreibenhaha');
insert into filePage(documentId, path, pageIndex, name) values(1, 'Users/1/labenwolf_zeugnis_small.pdf', 2, 'Labenwolf Zeugnis');
insert into filePage(documentId, path, pageIndex, name) values(1, 'Users/1/bewerbung_neu1.odt', 3, 'Anschreiben');
insert into filePage(documentId, path, pageIndex, name) values(1, 'Users/1/segitz_zeugnis_small.pdf', 4, 'Labenwolf Zeugnis');
insert into pageMap(documentId, pageIndex, key, value) values (1, 2, 'mainText', 'nur ein gruß');

insert into sentStatusValue(id, status) values(1, 'Waiting for reply after sending job application');
insert into sentStatusValue(id, status) values(2, 'Appointment for job interview');
insert into sentStatusValue(id, status) values(3, 'Job application rejected without an interview');
insert into sentStatusValue(id, status) values(4, 'Waiting for reply after job interview');
insert into sentStatusValue(id, status) values(5, 'Job application rejected after interview');
insert into sentStatusValue(id, status) values(6, 'Job application accepted after interview');

insert into sentDocumentEmail (subject, body) values ('subject', 'body');
insert into sentDocumentEmail (subject, body) values ('subject', 'body');
insert into sentUserValues(email, gender, degree, firstName, lastName, street, postcode, city, phone, mobilePhone)
             values('rene.ederer.nbg@gmail.com', 'm', '', 'René', 'Ederer', 'Raabstr. 24A', '90429', 'Nürnberg', 'meinTelefon', 'meinMobiltelefon');
insert into sentUserValues(email, gender, degree, firstName, lastName, street, postcode, city, phone, mobilePhone)
             values('rene.ederer.nbg@gmail.com', 'm', '', 'René', 'Ederer', 'Raabstr. 24A', '90429', 'Nürnberg', 'meinTelefon', 'meinMobiltelefon');
insert into employer(userId, company, street, postcode, city, gender, degree, firstName, lastName, email, phone, mobilePhone) values(1, 'BJC BEST JOB IT SERVICES GmbH', 'Alte Rabenstraße 32', '20148', 'Hamburg', 'f', '', 'Katrin', 'Thoms', 'Katrin.Thoms@bjc-its.de', '+49 (40) 5 14 00 7180', '');
insert into employer(userId, company, street, postcode, city, gender, degree, firstName, lastName, email, phone, mobilePhone) values(1, 'BJC BEST JOB IT SERVICES GmbH', 'Alte Rabenstraße 32', '20148', 'Hamburg', 'f', '', 'Der', 'Chef', 'Katrin.Thoms@bjc-its.de', '+49 (40) 5 14 00 7180', '');
insert into  sentDocument    (employerId, sentDocumentEmailId, sentUserValuesId, jobName, customVariables)
           values (1, 1, 1, 'Fachinformatiker', '');
insert into  sentDocument    (employerId, sentDocumentEmailId, sentUserValuesId, jobName, customVariables)
           values (2, 2, 2, 'Schiffskapitän', '');

insert into sentApplication(userId, sentDocumentId, url)
                      values(1, 1, 'meineUrl');
insert into sentApplication(userId, sentDocumentId, url)
                      values(1, 2, 'meineUrl');
insert into sentStatus(sentApplicationId, statusChangedOn, dueOn, sentStatusValueId, statusMessage)
                 values(1, current_date, null, 1, '');
insert into sentStatus(sentApplicationId, statusChangedOn, dueOn, sentStatusValueId, statusMessage)
                 values(2, current_date, null, 1, '');

/*
insert into sentApplication(userId, employerId, appliedAs) values(1, 1, 'Industriemechaniker');
insert into sentApplication(userId, employerId, appliedAs) values(1, 2, 'Informatiker');
insert into sentApplication(userId, employerId, appliedAs) values(1, 3, 'Bürokauffrau');

insert into sentStatus(sentApplicationId, statusChangedOn, dueOn, sentStatusValueId, statusMessage)
    values(1, to_timestamp('26.10.2017', '%d.%m.%Y'), null, 1, '');
insert into sentStatus(sentApplicationId, statusChangedOn, dueOn, sentStatusValueId, statusMessage)
    values(2, to_timestamp('26.10.2017', '%d.%m.%Y'), null, 1, '');
insert into sentStatus(sentApplicationId, statusChangedOn, dueOn, sentStatusValueId, statusMessage)
    values(3, to_timestamp('26.10.2017', '%d.%m.%Y'), null, 1, '');

*/




