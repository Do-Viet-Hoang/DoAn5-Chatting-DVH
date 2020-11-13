CREATE DATABASE Chating
CREATE TABLE Users
(
users_id uniqueidentifier not null DEFAULT newid() PRIMARY KEY,
email NVARCHAR(250),
password_hash VARBINARY(MAX) NOT NULL,
password_salt VARBINARY(MAX) NOT NULL,
full_name NVARCHAR(250) NOT NULL,
date_of_birth DATE,
addresss NVARCHAR(50),
SDT CHAR(25),
avatar VARCHAR(50),
roles BIT,
friends_counter INT,
active_flag BIT,
created_date_time DATETIME,
)
CREATE TABLE MessageGroup
(
message_group_id uniqueidentifier not null DEFAULT newid() PRIMARY KEY,
from_user_id UNIQUEIDENTIFIER FOREIGN KEY (from_user_id) REFERENCES Users(users_id),
to_user_id UNIQUEIDENTIFIER FOREIGN KEY (to_user_id) REFERENCES Users(users_id) ,
title NVARCHAR(250),
last_sending_datetime DATETIME,
last_message NVARCHAR(MAX),
mark_reading BIT,
active_flag BIT,
created_date_time DATETIME,
)
CREATE TABLE Meessages
(
Message_id uniqueidentifier not null DEFAULT newid() PRIMARY KEY,
Message_group_id UNIQUEIDENTIFIER FOREIGN KEY (Message_group_id) REFERENCES MessageGroup(Message_group_id),
from_user_id UNIQUEIDENTIFIER FOREIGN KEY (from_user_id) REFERENCES Users(users_id),
to_user_id UNIQUEIDENTIFIER FOREIGN KEY (to_user_id) REFERENCES Users(users_id),
name_message NVARCHAR(50),
media_flag INT,
content NVARCHAR(MAX),
media_file_path NVARCHAR(MAX),
active_flag INT,
created_date_time DATETIME,
)	
CREATE TABLE MessageGroupMedia
(
Message_media_id uniqueidentifier not null DEFAULT newid() PRIMARY KEY,
Message_group_id UNIQUEIDENTIFIER FOREIGN KEY (Message_group_id) REFERENCES MessageGroup(Message_group_id),
Message_id UNIQUEIDENTIFIER FOREIGN KEY (Message_id) REFERENCES Meessages(Message_id),
life_date_time DATETIME,
file_length BIGINT,
created_date_time DATETIME,
)	
CREATE TABLE MessageBox
(
Message_box_id uniqueidentifier not null DEFAULT newid() PRIMARY KEY,
from_user_id UNIQUEIDENTIFIER FOREIGN KEY (from_user_id) REFERENCES Users(users_id),
to_user_id UNIQUEIDENTIFIER FOREIGN KEY (to_user_id) REFERENCES Users(users_id),
)