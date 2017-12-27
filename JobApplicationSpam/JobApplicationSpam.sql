\c jobapplicationspam;

set client_encoding to 'UTF8';

drop table if exists htmlJobApplicationPageValue;
drop table if exists htmlJobApplicationPage;
drop table if exists htmlJobApplication;
drop table if exists htmlJobApplicationPageTemplate;
drop table if exists jobCenterContract;
drop table if exists jobApplicationStatus;
drop table if exists jobApplicationStatusValue;
drop table if exists jobApplication;
drop table if exists employer;
drop table if exists jobApplicationTemplateFile;
drop table if exists jobApplicationTemplate;
drop table if exists userValues;
drop table if exists users;

create table users (id serial primary key, email varchar(200) unique not null, password varchar(200) not null, salt varchar(200) not null, guid varchar(128) null);
create table userValues(userId int primary key, gender varchar(1) not null, degree varchar(20) not null, firstName varchar(50) not null, lastName varchar(50) not null, street varchar(50) not null, postcode varchar(20) not null, city varchar(50) not null, phone varchar(30) not null, mobilePhone varchar(30) not null, foreign key(userId) references users(id));
create table jobApplicationTemplate(id serial primary key, userId int not null, templateName varchar(100) not null, userAppliesAs varchar(200) not null, emailSubject varchar(100) not null, emailBody varchar(1000) not null, foreign key(userId) references users(id)); 
create table jobApplicationTemplateFile(id serial primary key, jobApplicationTemplateId int not null, filePath varchar(200) not null, foreign key(jobApplicationTemplateId) references jobApplicationTemplate(id));
create table employer(id serial primary key, userId int not null, company varchar(100) not null, street varchar(30) not null, postcode varchar(10) not null, city varchar(30) not null, gender varchar(20) not null, degree varchar(20) not null, firstName varchar(50) not null, lastName varchar(50) not null, email varchar(200) not null, phone varchar(30) not null, mobilePhone varchar(30) not null, foreign key(userId) references users(id));
create table jobApplication(id serial primary key, userId int not null, employerId int not null, jobApplicationTemplateId int not null, foreign key(jobApplicationTemplateId) references jobApplicationTemplate(id), foreign key(employerId) references employer(id), foreign key(userId) references users(id));
create table jobApplicationStatusValue(id int primary key, status varchar(50));
create table jobApplicationStatus(id serial primary key, jobApplicationId int, statusChangedOn date, dueOn timestamp, statusValueId int, statusMessage varchar(200), foreign key(jobApplicationId) references jobApplication(id), foreign key(statusValueId) references jobApplicationStatusValue(id));
create table jobCenterContract(id serial primary key, userId int not null, repeatEvery int not null, jobApplicationCount int not null, expireDate date not null, foreign key(userId) references users(id));
create table htmlJobApplicationPageTemplate(id serial primary key, name varchar(50), odtPath varchar(100));
create table htmlJobApplication(id serial primary key, userId int not null, name varchar(100) not null, foreign key(userId) references users(id));
create table htmlJobApplicationPage(id serial primary key, htmlJobApplicationId int not null, htmlJobApplicationPageTemplateId int not null, name varchar(50) not null, foreign key(htmlJobApplicationId) references htmlJobApplication(id), foreign key(htmlJobApplicationPageTemplateId) references htmlJobApplicationPageTemplate(id));
create table htmlJobApplicationPageValue(id serial primary key, htmlJobApplicationPageId int not null, key varchar(100) not null, value text not null, foreign key(htmlJobApplicationPageId) references htmlJobApplicationPage(id));

insert into users(email, password, salt, guid) values('rene', '1234', 'salt', null);
insert into users(email, password, salt, guid) values('rene.ederer.nbg@gmail.com', 'r99n/4/4NGGeD7pn4I1STI2rI+BFweUmzAqkxwLUzFP9aB7g4zR5CBHx+Nz2yn3NbiY7/plf4ZRGPaXXnQvFsA==', 'JjjYQTWgutm4pv/VnzgHf6r4NjNrAVcTq+xnR7/JsRGAIHRdrcw3IMVrzngn2KPRakfX/S1kl9VrqwAT+T02Og==', null);

insert into userValues(userId, gender, degree, firstName, lastName, street, postcode, city, phone, mobilePhone) values(1, 'm', '', 'rene', 'ederer', 'taabsrr. 24A', '90429', 'Nürnberg', '', '01520 2723494');
insert into userValues(userId, gender, degree, firstName, lastName, street, postcode, city, phone, mobilePhone) values(2, 'm', '', 'René', 'Ederer', 'Raabstr. 24A', '90429', 'Nürnberg', '', '01520 2723494');

insert into jobApplicationTemplate(userId, templateName, userAppliesAs, emailSubject, emailBody) values(2, 'Mein Template', 'Fachinformatiker für Anwendungsentwicklung', 'Bewerbung als Fachinformatiker für Anwendungsentwicklung', 'Sehr $geehrter $chefAnrede $chefNachname,\n\nanbei schicke ich Ihnen meine Bewerbungsunterlagen.\nDas Jobcenter kann während der Einarbeitungszeit (auch mehrere Monate) bis zu 50% der Gehaltskosten übernehmen.\nMeine Sachbearbeiterin Frau Götz (jobcenter-nuernberg-stadt.mitte-ag-team@jobcenter-ge.de) gibt Ihnen gerne nähere Auskunft.\nÜber eine Einladung zu einem Bewerbungsgespräch würder ich mich sehr freuen.\n\nMit freundlichen Grüßen\n\n\n$meinVorname $meinNachname\n$meineStrasse\n$meinePlz $meineStadt\n$meineMobilnr');
insert into jobApplicationTemplate(userId, templateName, userAppliesAs, emailSubject, emailBody) values(2, 'Mein Template ohne Anhang', 'Fachinformatiker für Anwendungsentwicklung', 'Bewerbung als Fachinformatiker für Anwendungsentwicklung', 'Sehr $geehrter $chefAnrede $chefNachname,\n\nanbei schicke ich Ihnen meine Bewerbungsunterlagen.\nDas Jobcenter kann während der Einarbeitungszeit (auch mehrere Monate) bis zu 50% der Gehaltskosten übernehmen.\nMeine Sachbearbeiterin Frau Götz (jobcenter-nuernberg-stadt.mitte-ag-team@jobcenter-ge.de) gibt Ihnen gerne nähere Auskunft.\nÜber eine Einladung zu einem Bewerbungsgespräch würder ich mich sehr freuen.\n\nMit freundlichen Grüßen\n\n\n$meinVorname $meinNachname\n$meineStrasse\n$meinePlz $meineStadt\n$meineMobilnr');

insert into jobApplicationTemplateFile (jobApplicationTemplateId, filePath) values(1, 'C:/Users/rene/Desktop/bewerbung_neu.odt');
insert into jobApplicationTemplateFile (jobApplicationTemplateId, filePath) values(1, 'c:/users/rene/Downloads/ihk_zeugnis_small.pdf');
insert into jobApplicationTemplateFile (jobApplicationTemplateId, filePath) values(1, 'c:/users/rene/Downloads/segitz_zeugnis_small.pdf');
insert into jobApplicationTemplateFile (jobApplicationTemplateId, filePath) values(1, 'c:/users/rene/Downloads/kmk_zeugnis_small.pdf');
insert into jobApplicationTemplateFile (jobApplicationTemplateId, filePath) values(1, 'c:/users/rene/Downloads/labenwolf_zeugnis_small.pdf');

insert into jobApplicationTemplateFile(jobApplicationTemplateId, filePath) values(2, 'C:/Users/rene/Desktop/bewerbung_neu.odt');

insert into employer(userId, company, street, postcode, city, gender, degree, firstName, lastName, email, phone, mobilePhone) values(1, 'BJC BEST JOB IT SERVICES GmbH', 'Alte Rabenstraße 32', '20148', 'Hamburg', 'f', '', 'Katrin', 'Thoms', 'Katrin.Thoms@bjc-its.de', '+49 (40) 5 14 00 7180', '');
insert into employer(userId, company, street, postcode, city, gender, degree, firstName, lastName, email, phone, mobilePhone) values(1, 'Deutsche Anwaltshotline AG', 'Am Plärrer 7', '90443', 'Nürnberg', 'm', '', 'Jonas', 'Zimmermann', 'mail@deutsche-anwaltshotline.de', '+49 911 3765690', '');
insert into employer(userId, company, street, postcode, city, gender, degree, firstName, lastName, email, phone, mobilePhone) values(1, 'ANG.-Punkt und gut! GmbH', 'Südwestpark 37-41', '90449', 'Nürnberg', 'f', '', 'Jaqueline', 'Strauß', 'bewerbung@ang.de', '+49 911 525700', '+49 1778876348');
insert into employer(userId, company, street, postcode, city, gender, degree, firstName, lastName, email, phone, mobilePhone) values(1, 'iQ-Bewerbermanagement', 'Obernstr. 111', '28832', 'Achim bei Bremen', 'f', '', 'Nele', 'Sommerfeld', 'nele.sommerfeld@iq-bewerbermanagement.de', '+49 40 6003852232', '');
insert into employer(userId, company, street, postcode, city, gender, degree, firstName, lastName, email, phone, mobilePhone) values(1, 'engineering people GmbH', 'Südwestpark 60', '90449', 'Nürnberg',  'm', '', 'Haluk', 'Acar','haluk.acar@engineering-people.de', '+49 911 239560316', '');
insert into employer(userId, company, gender, street, postcode, city, degree, firstName, lastName, email, phone, mobilePhone) values(1, 'BFI Informationssysteme GmbH', 'Ötterichweg 7', '90411', 'Nürnberg', 'm', '', 'Michael', 'Schlund', 'Michael.Schlund@bfi-info.de', '0911 9457668', '');


insert into jobApplicationStatusValue(id, status) values(1, 'Waiting for reply after sending job application');
insert into jobApplicationStatusValue(id, status) values(2, 'Appointment for job interview');
insert into jobApplicationStatusValue(id, status) values(3, 'Job application rejected without an interview');
insert into jobApplicationStatusValue(id, status) values(4, 'Waiting for reply after job interview');
insert into jobApplicationStatusValue(id, status) values(5, 'Job application rejected after interview');
insert into jobApplicationStatusValue(id, status) values(6, 'Job application accepted after interview');

insert into jobApplication(userId, employerId, jobApplicationTemplateId) values(1, 1, 1);
insert into jobApplication(userId, employerId, jobApplicationTemplateId) values(1, 2, 1);
insert into jobApplication(userId, employerId, jobApplicationTemplateId) values(1, 3, 1);
insert into jobApplication(userId, employerId, jobApplicationTemplateId) values(1, 4, 1);
insert into jobApplication(userId, employerId, jobApplicationTemplateId) values(1, 5, 1);
insert into jobApplication(userId, employerId, jobApplicationTemplateId) values(1, 6, 1);

insert into jobApplicationStatus(jobApplicationId, statusChangedOn, dueOn, statusValueId, statusMessage)
    values(1, to_timestamp('26.10.2017', '%d.%m.%Y'), null, 1, '');
insert into jobApplicationStatus(jobApplicationId, statusChangedOn, dueOn, statusValueId, statusMessage)
    values(2, to_timestamp('26.10.2017', '%d.%m.%Y'), null, 1, '');
insert into jobApplicationStatus(jobApplicationId, statusChangedOn, dueOn, statusValueId, statusMessage)
    values(3, to_timestamp('26.10.2017', '%d.%m.%Y'), null, 1, '');
insert into jobApplicationStatus(jobApplicationId, statusChangedOn, dueOn, statusValueId, statusMessage)
    values(4, to_timestamp('26.10.2017', '%d.%m.%Y'), null, 1, '');
insert into jobApplicationStatus(jobApplicationId, statusChangedOn, dueOn, statusValueId, statusMessage)
    values(5, to_timestamp('26.01.2017', '%d.%m.%Y'), null, 1, '');
insert into jobApplicationStatus(jobApplicationId, statusChangedOn, dueOn, statusValueId, statusMessage)
    values(6, to_timestamp('26.10.2017', '%d.%m.%Y'), null, 1, 'Forwarded by Ms Götz');


insert into htmlJobApplicationPageTemplate(name, odtPath) values('Anschreiben nach DIN 5008', 'c:/users/rene/desktop/bewerbung_neu.odt');
insert into htmlJobApplication(userId, name) values(2, 'mein htmlTemplate');
insert into htmlJobApplicationPage (htmlJobApplicationId, htmlJobApplicationPageTemplateId, name) values(1, 1, 'mein Anschreiben');
insert into htmlJobApplicationPageValue(htmlJobApplicationPageId, key, value) values (1, 'mainText', 'Sehr geehrte Damen und Herren\n\nhiermit bewerbe ich mich auf Ihre Stellenzeige\nauf LinkedIn\n\nMit freundlichen Grüßen\n\n\n\nRené Ederer');










