

-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2020-03-22 07:12:20.376

-- tables
-- Table: Enrollment
CREATE TABLE Enrollment (
    IdEnrollment int  NOT NULL,
    Semester int  NOT NULL,
    IdStudy int  NOT NULL,
    StartDate date  NOT NULL,
    CONSTRAINT Enrollment_pk PRIMARY KEY  (IdEnrollment)
);

-- Table: Student
CREATE TABLE Student (
    IndexNumber nvarchar(100)  NOT NULL,
    FirstName nvarchar(100)  NOT NULL,
    LastName nvarchar(100)  NOT NULL,
    Password nvarchar(100) NOT NULL,
    BirthDate date  NOT NULL,
    IdEnrollment int  NOT NULL,
    CONSTRAINT Student_pk PRIMARY KEY (IndexNumber)
);

-- Table: Studies
CREATE TABLE Studies (
    IdStudy int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    CONSTRAINT Studies_pk PRIMARY KEY  (IdStudy)
);

-- foreign keys
-- Reference: Enrollment_Studies (table: Enrollment)
ALTER TABLE Enrollment ADD CONSTRAINT Enrollment_Studies
    FOREIGN KEY (IdStudy)
    REFERENCES Studies (IdStudy);

-- Reference: Student_Enrollment (table: Student)
ALTER TABLE Student ADD CONSTRAINT Student_Enrollment
    FOREIGN KEY (IdEnrollment)
    REFERENCES Enrollment (IdEnrollment);

-- End of file.

INSERT INTO Studies(IdStudy, Name) VALUES(1, 'IT');
INSERT INTO Studies(IdStudy, Name) VALUES(2, 'Matematyka');
INSERT INTO Studies(IdStudy, Name) VALUES(3, 'Fizyka');

INSERT INTO Enrollment(IdEnrollment, IdStudy, Semester, StartDate) VALUES(1, 1, 1, '2020-01-01');

INSERT INTO Enrollment(IdEnrollment, IdStudy, Semester, StartDate) VALUES(2, 2, 1, '2020-01-01');
INSERT INTO Enrollment(IdEnrollment, IdStudy, Semester, StartDate) VALUES(3, 3, 2, '2020-06-01');
INSERT INTO Enrollment(IdEnrollment, IdStudy, Semester, StartDate) VALUES(4, 1, 3, '2019-01-01');

INSERT INTO Student(IndexNumber, FirstName, LastName, Password, BirthDate, IdEnrollment) VALUES('s100', 'John', 'Doe', 'johndoe', '1990-06-01', 1)
INSERT INTO Student(IndexNumber, FirstName, LastName, Password, BirthDate, IdEnrollment) VALUES('s101', 'Mike', 'Doe', 'mikedoe', '1990-06-01', 1)
INSERT INTO Student(IndexNumber, FirstName, LastName, Password, BirthDate, IdEnrollment) VALUES('s102', 'Alice', 'Doe', 'alicedoe', '1990-06-01', 2)
INSERT INTO Student(IndexNumber, FirstName, LastName, Password, BirthDate, IdEnrollment) VALUES('s103', 'Alice', 'Mike', 'alicemike', '1990-06-01', 3)