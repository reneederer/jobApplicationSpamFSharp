﻿;set client_encoding to 'UTF8';
drop table if exists sentStatus cascade;
drop table if exists sentStatusValue cascade;
drop table if exists sentApplication cascade;
drop table if exists pageMap cascade;
drop table if exists lastEditedDocumentId cascade;
drop table if exists htmlPage cascade;
drop table if exists filePage cascade;
drop table if exists documentEmail cascade;
drop table if exists document cascade;
drop table if exists htmlPageTemplate cascade;
drop table if exists employer cascade;
drop table if exists userValues cascade;
drop table if exists users cascade;

create table users (id serial primary key, email varchar(200) unique not null, password varchar(200) not null, salt varchar(200) not null, guid varchar(128) null);
create table userValues(id serial primary key, userId int unique not null, gender varchar(1) not null, degree varchar(20) not null, firstName varchar(50) not null, lastName varchar(50) not null, street varchar(50) not null, postcode varchar(20) not null, city varchar(50) not null, phone varchar(30) not null, mobilePhone varchar(30) not null, foreign key(userId) references users(id));
create table employer(id serial primary key, userId int not null, company varchar(100) not null, street varchar(30) not null, postcode varchar(10) not null, city varchar(30) not null, gender varchar(20) not null, degree varchar(20) not null, firstName varchar(50) not null, lastName varchar(50) not null, email varchar(200) not null, phone varchar(30) not null, mobilePhone varchar(30) not null, foreign key(userId) references users(id));
create table htmlPageTemplate(id serial primary key, name varchar(50) unique not null, odtPath varchar(100) unique not null, html text not null);
create table document(id serial primary key, userId int not null, name varchar(100) not null, foreign key(userId) references users(id));
create table lastEditedDocumentId(userId int unique primary key not null, documentId int not null, foreign key(userId) references users(id), foreign key(documentId) references document(id));
create table filePage(id serial primary key, documentId int not null, path varchar(60) not null, pageIndex int not null, name varchar(50) not null, foreign key(documentId) references document(id), constraint filePage_unique unique(documentId, pageIndex));
create table htmlPage(id serial primary key, documentId int not null, templateId int null, pageIndex int not null, name varchar(50) not null, foreign key(documentId) references document(id), foreign key(templateId) references htmlPageTemplate(id), constraint htmlPage_unique unique(documentId, pageIndex));
create table documentEmail(id serial primary key, documentId int not null unique, subject varchar(200) not null, body text not null, foreign key(documentId) references document(id));
create table pageMap(id serial primary key, documentId int not null, pageIndex int not null, key varchar(100) not null, value text not null, foreign key(documentId) references document(id), constraint pageMap_unique unique(documentId, pageIndex, key));
create table sentApplication(id serial primary key, userId int not null, employerId int not null, documentId int null, foreign key(employerId) references employer(id), foreign key(userId) references users(id));
create table sentStatusValue(id int primary key, status varchar(50));
create table sentStatus(id serial primary key, sentApplicationId int, statusChangedOn date, dueOn timestamp, sentStatusValueId int, statusMessage varchar(200), foreign key(sentApplicationId) references sentApplication(id), foreign key(sentStatusValueId) references sentStatusValue(id));

insert into users(email, password, salt, guid) values('rene.ederer.nbg@gmail.com', 'r99n/4/4NGGeD7pn4I1STI2rI+BFweUmzAqkxwLUzFP9aB7g4zR5CBHx+Nz2yn3NbiY7/plf4ZRGPaXXnQvFsA==', 'JjjYQTWgutm4pv/VnzgHf6r4NjNrAVcTq+xnR7/JsRGAIHRdrcw3IMVrzngn2KPRakfX/S1kl9VrqwAT+T02Og==', null);
insert into users(email, password, salt, guid) values('helmut.goerke@gmail.com', 'r99n/4/4NGGeD7pn4I1STI2rI+BFweUmzAqkxwLUzFP9aB7g4zR5CBHx+Nz2yn3NbiY7/plf4ZRGPaXXnQvFsA==', 'JjjYQTWgutm4pv/VnzgHf6r4NjNrAVcTq+xnR7/JsRGAIHRdrcw3IMVrzngn2KPRakfX/S1kl9VrqwAT+T02Og==', 'someguid');
insert into users(email, password, salt, guid) values('r', 'r99n/4/4NGGeD7pn4I1STI2rI+BFweUmzAqkxwLUzFP9aB7g4zR5CBHx+Nz2yn3NbiY7/plf4ZRGPaXXnQvFsA==', 'JjjYQTWgutm4pv/VnzgHf6r4NjNrAVcTq+xnR7/JsRGAIHRdrcw3IMVrzngn2KPRakfX/S1kl9VrqwAT+T02Og==', null);

insert into userValues(userId, gender, degree, firstName, lastName, street, postcode, city, phone, mobilePhone) values(1, 'm', '', 'René', 'Ederer', 'Raabstr. 24A', '90429', 'Nürnberg', 'kein Telefon', 'kein Handy');
insert into userValues(userId, gender, degree, firstName, lastName, street, postcode, city, phone, mobilePhone) values(2, 'm', '', 'Helmut', 'Görke', 'Raabstr. 24A', '90429', 'Nürnberg', '0911 918273', '01520 2723494');

insert into employer(userId, company, street, postcode, city, gender, degree, firstName, lastName, email, phone, mobilePhone) values(1, 'BJC BEST JOB IT SERVICES GmbH', 'Alte Rabenstraße 32', '20148', 'Hamburg', 'f', '', 'Katrin', 'Thoms', 'Katrin.Thoms@bjc-its.de', '+49 (40) 5 14 00 7180', '');
insert into employer(userId, company, street, postcode, city, gender, degree, firstName, lastName, email, phone, mobilePhone) values(1, 'Deutsche Anwaltshotline AG', 'Am Plärrer 7', '90443', 'Nürnberg', 'm', '', 'Jonas', 'Zimmermann', 'mail@deutsche-anwaltshotline.de', '+49 911 3765690', '');
insert into employer(userId, company, street, postcode, city, gender, degree, firstName, lastName, email, phone, mobilePhone) values(1, 'ANG.-Punkt und gut! GmbH', 'Südwestpark 37-41', '90449', 'Nürnberg', 'f', '', 'Jaqueline', 'Strauß', 'bewerbung@ang.de', '+49 911 525700', '+49 1778876348');
insert into employer(userId, company, street, postcode, city, gender, degree, firstName, lastName, email, phone, mobilePhone) values(1, 'iQ-Bewerbermanagement', 'Obernstr. 111', '28832', 'Achim bei Bremen', 'f', '', 'Nele', 'Sommerfeld', 'nele.sommerfeld@iq-bewerbermanagement.de', '+49 40 6003852232', '');
insert into employer(userId, company, street, postcode, city, gender, degree, firstName, lastName, email, phone, mobilePhone) values(1, 'engineering people GmbH', 'Südwestpark 60', '90449', 'Nürnberg',  'm', '', 'Haluk', 'Acar','haluk.acar@engineering-people.de', '+49 911 239560316', '');
insert into employer(userId, company, gender, street, postcode, city, degree, firstName, lastName, email, phone, mobilePhone) values(1, 'BFI Informationssysteme GmbH', 'Ötterichweg 7', '90411', 'Nürnberg', 'm', '', 'Michael', 'Schlund', 'Michael.Schlund@bfi-info.de', '0911 9457668', '');

insert into htmlPageTemplate(name, odtPath, html) values('Anschreiben nach DIN 5008', 'c:/users/rene/desktop/bewerbung_neu.odt',
'<div id="insertDiv">
<div id="divTemplate" class="page1">
    <div style="width: 100%; background-color: white">
        <input class="resizing" autofocus "autofocus" style="font-family: Arial; font-size: 12pt; font-weight: normal" data-bind="userDegree" data-variable-value="userDegree" placeholder="Dein Titel" />
        <input class="resizing" value="test" style="border:none; outline: none; font-family: Arial; font-size: 12pt; font-weight: normal"  data-variable-value="userFirstName" data-bind="userFirstName" placeholder="Dein Vorname" />
        <input class="resizing" style="border:none; outline: none; letter-spacing: 0px; font-family: Arial; font-size: 12pt; font-weight: normal"  data-bind="userLastName" data-variable-value="userLasssstName" placeholder="Dein Nachname" />
        <br />
        <input class="resizing" style="border:none; outline: none; font-family: Arial; font-size: 12pt; font-weight: normal"  data-bind="userStreet" style= "width:150px" data-variable-value="userStreet" placeholder="Deine Straße" />
        <br />
        <input class="resizing" style="border:none; outline: none; font-family: Arial; font-size: 12pt; font-weight: normal"  data-bind="userPostcode" data-variable-value="userPostcode" placeholder="Deine Postleitzahl" />
        <input class="resizing" style="border:none; outline: none; font-family: Arial; font-size: 12pt; font-weight: normal"  data-bind="userCity" data-variable-value="userCity" placeholder="Deine Stadt" />
        <br />
        <br />
        <br />
        <br />
        <input class="resizing" style="border:none; outline: none; font-family: Arial; font-size: 12pt; font-weight: normal"  data-bind="bossDegree" placeholder="Chef-Titel" />
        <input class="resizing" style="border:none; outline: none; font-family: Arial; font-size: 12pt; font-weight: normal"  data-bind="bossFirstName" placeholder="Chef-Vorname" />
        <input class="resizing" style="border:none; outline: none; font-family: Arial; font-size: 12pt; font-weight: normal"  data-bind="bossLastName" placeholder="Chef-Nachname" />
        <br />
        <input class="resizing" style="border:none; outline: none; font-family: Arial; font-size: 12pt; font-weight: normal"  data-bind="companyStreet" placeholder="Firma-Strasse" />
        <br />
        <input class="resizing" style="border:none; outline: none; font-family: Arial; font-size: 12pt; font-weight: normal"  data-bind="companyPostcode" placeholder="Firma-Postleitzahl" />
        <input class="resizing" style="border:none; outline: none; font-family: Arial; font-size: 12pt; font-weight: normal"  data-bind="companyCity" placeholder="Firma-Stadt" />
        <br />
<span style= "float:right">
<input type="text" readonly="readonly" class="resizing" style="font-family: Arial; font-size: 12pt; font-weight: normal; border: none; outline: none;padding:0px; margin:0px" data-bind="userCity" />,&nbsp
<input type="text" readonly="readonly" class="resizing" style="font-family: Arial; font-size: 12pt; font-weight: normal" data-variable-value="today" />
</span>
        <br />
        <br />
        <input class="resizing" data-bind="subject" style="border:none; outline: none; font-family: Arial; font-size: 12pt; font-weight: bold" placeholder="Betreff" />
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
<input type="text" readonly="readonly" class="resizing" style="border: none; outline: none;font-family: Arial; font-size: 12pt; font-weight: normal" data-bind="userDegree" />&nbsp;
<input type="text" readonly="readonly" class="resizing" style="border: none; outline: none;font-family: Arial; font-size: 12pt; font-weight: normal" data-bind="userFirstName" />&nbsp;
<input type="text" readonly="readonly" class="resizing" style="border: none; outline: none;font-family: Arial; font-size: 12pt; font-weight: normal" data-bind="userLastName" />
</div>
</div>
</div>');
insert into htmlPageTemplate(name, odtPath, html) values('Deckblatt', 'c:/users/rene/desktop/bewerbung_deckblatt.odt',
'<div id="insertDiv">
<h1>Deckblatt</h1>
<input class="resizing" style="border:none; outline: none; font-family: Arial; font-size: 12pt; font-weight: normal" data-bind="bossFirstName" placeholder="Chef-Vorname" />
<image src="null" width="400" height="100" />
<input type="text" class="resizing" data-bind="userFirstName"></input>
<input type="text" data-page-mainText="xainText"></input>
<br />
<input type="text" class="resizing" style="border: none; outline: none;font-family: Arial; font-size: 12pt; font-weight: normal" data-bind="userLastName" placeholder="Dein Name" />
hallo div!</div>
');
insert into htmlPageTemplate(name, odtPath, html) values('Lebenslauf', 'c:/users/rene/desktop/bewerbung_lebenslauf.odt', '<div id="insertDiv"><b>Lebenslauf...</b></div>');
insert into document(userId, name) values(1, 'mein htmlTemplate');
insert into document(userId, name) values(1, 'mein zweites htmlTemplate');
insert into documentEmail(documentId, subject, body) values(1, 'titel1', 'body1');
insert into documentEmail(documentId, subject, body) values(2, 'titel2', 'body2');
insert into filePage(documentId, path, pageIndex, name) values(1, 'C:/Users/rene/Downloads/labenwolf_zeugnis_small.pdf', 3, 'Labenwolf Zeugnis');
insert into htmlPage(documentId, templateId, pageIndex, name) values(2, 1, 1, 'mein zweites Anschreiben');
insert into htmlPage(documentId, templateId, pageIndex, name) values(2, 1, 2, 'mein zweites Deckblatt');
insert into htmlPage(documentId, templateId, pageIndex, name) values(2, 2, 3, 'mein drittes Deckblatt');
insert into htmlPage(documentId, templateId, pageIndex, name) values(2, 3, 4, 'mein dritter Lebenslauf');
insert into filePage(documentId, path, pageIndex, name) values(2, 'C:/Users/rene/Downloads/labenwolf_zeugnis_small.pdf', 5, 'LabenwolfZeugnis');
insert into htmlPage(documentId, templateId, pageIndex, name) values(1, 1, 1, 'mein Anschreiben');
insert into htmlPage(documentId, templateId, pageIndex, name) values(1, 1, 2, 'mein Deckblatt');
insert into pageMap(documentId, pageIndex, key, value) values (1, 1, 'mainText', 'Sehr geehrte Damen und Herren\n\nhiermit bewerbe ich mich auf Ihre Stellenzeige\nauf LinkedIn\n\nMit freundlichen Grüßen\n\n\n\nRené Ederer');
insert into pageMap(documentId, pageIndex, key, value) values (1, 2, 'mainText', 'nur ein gruß');

insert into sentStatusValue(id, status) values(1, 'Waiting for reply after sending job application');
insert into sentStatusValue(id, status) values(2, 'Appointment for job interview');
insert into sentStatusValue(id, status) values(3, 'Job application rejected without an interview');
insert into sentStatusValue(id, status) values(4, 'Waiting for reply after job interview');
insert into sentStatusValue(id, status) values(5, 'Job application rejected after interview');
insert into sentStatusValue(id, status) values(6, 'Job application accepted after interview');

insert into sentApplication(userId, employerId, documentId) values(1, 1, 1);
insert into sentApplication(userId, employerId, documentId) values(1, 2, 1);
insert into sentApplication(userId, employerId, documentId) values(1, 3, 1);

insert into sentStatus(sentApplicationId, statusChangedOn, dueOn, sentStatusValueId, statusMessage)
    values(1, to_timestamp('26.10.2017', '%d.%m.%Y'), null, 1, '');
insert into sentStatus(sentApplicationId, statusChangedOn, dueOn, sentStatusValueId, statusMessage)
    values(2, to_timestamp('26.10.2017', '%d.%m.%Y'), null, 1, '');
insert into sentStatus(sentApplicationId, statusChangedOn, dueOn, sentStatusValueId, statusMessage)
    values(3, to_timestamp('26.10.2017', '%d.%m.%Y'), null, 1, '');


