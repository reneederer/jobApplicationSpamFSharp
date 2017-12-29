;set client_encoding to 'UTF8';
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
create table userValues(id serial primary key, userId int, gender varchar(1) not null, degree varchar(20) not null, firstName varchar(50) not null, lastName varchar(50) not null, street varchar(50) not null, postcode varchar(20) not null, city varchar(50) not null, phone varchar(30) not null, mobilePhone varchar(30) not null, foreign key(userId) references users(id));
create table employer(id serial primary key, userId int not null, company varchar(100) not null, street varchar(30) not null, postcode varchar(10) not null, city varchar(30) not null, gender varchar(20) not null, degree varchar(20) not null, firstName varchar(50) not null, lastName varchar(50) not null, email varchar(200) not null, phone varchar(30) not null, mobilePhone varchar(30) not null, foreign key(userId) references users(id));
create table htmlJobApplicationPageTemplate(id serial primary key, name varchar(50), odtPath varchar(100));
create table htmlJobApplication(id serial primary key, userId int not null, name varchar(100) not null, foreign key(userId) references users(id));
create table htmlJobApplicationPage(id serial primary key, htmlJobApplicationId int not null, htmlJobApplicationPageTemplateId int not null, name varchar(50) not null, foreign key(htmlJobApplicationId) references htmlJobApplication(id), foreign key(htmlJobApplicationPageTemplateId) references htmlJobApplicationPageTemplate(id));
create table htmlJobApplicationPageValue(id serial primary key, htmlJobApplicationPageId int not null, key varchar(100) not null, value text not null, foreign key(htmlJobApplicationPageId) references htmlJobApplicationPage(id));
create table jobApplication(id serial primary key, userId int not null, employerId int not null, htmlJobApplicationId int not null, foreign key(htmlJobApplicationId) references htmlJobApplicationId) references employer(id), foreign key(userId) references users(id));
create table jobApplicationStatusValue(id int primary key, status varchar(50));
create table jobApplicationStatus(id serial primary key, jobApplicationId int, statusChangedOn date, dueOn timestamp, statusValueId int, statusMessage varchar(200), foreign key(jobApplicationId) references jobApplication(id), foreign key(statusValueId) references jobApplicationStatusValue(id));

insert into users(email, password, salt, guid) values('rene.ederer.nbg@gmail.com', 'r99n/4/4NGGeD7pn4I1STI2rI+BFweUmzAqkxwLUzFP9aB7g4zR5CBHx+Nz2yn3NbiY7/plf4ZRGPaXXnQvFsA==', 'JjjYQTWgutm4pv/VnzgHf6r4NjNrAVcTq+xnR7/JsRGAIHRdrcw3IMVrzngn2KPRakfX/S1kl9VrqwAT+T02Og==', null);
insert into users(email, password, salt, guid) values('helmut.goerke@gmail.com', 'r99n/4/4NGGeD7pn4I1STI2rI+BFweUmzAqkxwLUzFP9aB7g4zR5CBHx+Nz2yn3NbiY7/plf4ZRGPaXXnQvFsA==', 'JjjYQTWgutm4pv/VnzgHf6r4NjNrAVcTq+xnR7/JsRGAIHRdrcw3IMVrzngn2KPRakfX/S1kl9VrqwAT+T02Og==', 'someguid');

insert into userValues(userId, gender, degree, firstName, lastName, street, postcode, city, phone, mobilePhone) values(1, 'm', '', 'René', 'Ederer', 'Raabstr. 24A', '90429', 'Nürnberg', 'kein Telefon', 'kein Handy');
insert into userValues(userId, gender, degree, firstName, lastName, street, postcode, city, phone, mobilePhone) values(2, 'm', '', 'Helmut', 'Görke', 'Raabstr. 24A', '90429', 'Nürnberg', '', '01520 2723494');

insert into employer(userId, company, street, postcode, city, gender, degree, firstName, lastName, email, phone, mobilePhone) values(1, 'BJC BEST JOB IT SERVICES GmbH', 'Alte Rabenstraße 32', '20148', 'Hamburg', 'f', '', 'Katrin', 'Thoms', 'Katrin.Thoms@bjc-its.de', '+49 (40) 5 14 00 7180', '');
insert into employer(userId, company, street, postcode, city, gender, degree, firstName, lastName, email, phone, mobilePhone) values(1, 'iQ-Bewerbermanagement', 'Obernstr. 111', '28832', 'Achim bei Bremen', 'f', '', 'Nele', 'Sommerfeld', 'nele.sommerfeld@iq-bewerbermanagement.de', '+49 40 6003852232', '');
insert into employer(userId, company, street, postcode, city, gender, degree, firstName, lastName, email, phone, mobilePhone) values(1, 'engineering people GmbH', 'Südwestpark 60', '90449', 'Nürnberg',  'm', '', 'Haluk', 'Acar','haluk.acar@engineering-people.de', '+49 911 239560316', '');

insert into jobApplicationStatusValue(id, status) values(1, 'Waiting for reply after sending job application');
insert into jobApplicationStatusValue(id, status) values(2, 'Appointment for job interview');
insert into jobApplicationStatusValue(id, status) values(3, 'Job application rejected without an interview');
insert into jobApplicationStatusValue(id, status) values(4, 'Waiting for reply after job interview');
insert into jobApplicationStatusValue(id, status) values(5, 'Job application rejected after interview');
insert into jobApplicationStatusValue(id, status) values(6, 'Job application accepted after interview');

insert into jobApplication(userId, employerId, jobApplicationTemplateId) values(1, 1, 1);
insert into jobApplication(userId, employerId, jobApplicationTemplateId) values(1, 2, 1);
insert into jobApplication(userId, employerId, jobApplicationTemplateId) values(1, 3, 1);

insert into jobApplicationStatus(jobApplicationId, statusChangedOn, dueOn, statusValueId, statusMessage)
    values(1, to_timestamp('26.10.2017', '%d.%m.%Y'), null, 1, '');
insert into jobApplicationStatus(jobApplicationId, statusChangedOn, dueOn, statusValueId, statusMessage)
    values(2, to_timestamp('26.10.2017', '%d.%m.%Y'), null, 1, '');
insert into jobApplicationStatus(jobApplicationId, statusChangedOn, dueOn, statusValueId, statusMessage)
    values(3, to_timestamp('26.10.2017', '%d.%m.%Y'), null, 1, '');


insert into htmlJobApplicationPageTemplate(name, odtPath) values('Anschreiben nach DIN 5008', 'c:/users/rene/desktop/bewerbung_neu.odt');
insert into htmlJobApplication(userId, name) values(2, 'mein htmlTemplate');
insert into htmlJobApplicationPage (htmlJobApplicationId, htmlJobApplicationPageTemplateId, name) values(1, 1, 'mein Anschreiben');
insert into htmlJobApplicationPageValue(htmlJobApplicationPageId, key, value) values (1, 'mainText', 'Sehr geehrte Damen und Herren\n\nhiermit bewerbe ich mich auf Ihre Stellenzeige\nauf LinkedIn\n\nMit freundlichen Grüßen\n\n\n\nRené Ederer');










