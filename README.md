YOUTUBE VIDEO LINK ATTACHED(FOR DEMONSTRATION) - Part 1 : https://youtu.be/ueTd_q9qGL8
YOUTUBE VIDEO LINK ATTACHED(FOR DEMONSTRATION) - Part 2 : https://www.youtube.com/watch?v=lNGZRolHSRo
YOUTUBE VIDEO LINK ATTACHED(FOR PRESENTATION)  - Part 3 : https://youtu.be/06A5RZ4uJkY

EVENT EASE MVC APPLICATION
SCRIPT FOR EVENTEASEDB:
-- Creating my DB--
CREATE DATABASE EventEaseDB;
Go

USE EventEaseDB;
GO

CREATE TABLE Venue (
   VENUEID INT IDENTITY(1,1) PRIMARY KEY,
   VENUENAME VARCHAR(60) NOT NULL,
   ADDRESS VARCHAR(255) NOT NULL,
   CAPACITY INT NOT NULL,
   LOCATION VARCHAR(50) NOT NULL,
   IMAGE_URL VARCHAR(200),
   CONSTRAINT CK_Venue_Capacity_Positive CHECK (CAPACITY > 0)
);
Go

CREATE TABLE Events (
    EVENT_ID INT IDENTITY(1,1) PRIMARY KEY,
    EVENT_NAME VARCHAR(50) NOT NULL,
    EVENT_DESCRIPTION VARCHAR(100),
    EVENT_TIME DATETIME2 NOT NULL,
    EVENT_END_TIME DATETIME2,
    LOCATION VARCHAR(100),
    STATUS VARCHAR(50) DEFAULT 'Scheduled'
    CONSTRAINT CK_Event_Status CHECK (STATUS IN ('Scheduled', 'Ongoing', 'Completed', 'Cancelled'))
);
GO

CREATE TABLE Bookings (
    BOOKINGID INT IDENTITY(1,1) PRIMARY KEY,
    BOOKINGREFERENCE VARCHAR(20) UNIQUE NOT NULL,
    BOOKING_DATE DATETIME2 DEFAULT GETDATE(),
    BOOKING_STATUS VARCHAR(50) DEFAULT 'Pending',
    VENUE_ID INT NOT NULL,
    EVENTS_ID INT NOT NULL,
    CUSTOMER_NAME VARCHAR(100) NOT NULL,
    CUSTOMER_CONTACT VARCHAR(50) NOT NULL,
    
    -- Foreign Key Constraints
    CONSTRAINT FK_Bookings_Venue FOREIGN KEY (VENUE_ID) 
        REFERENCES Venue(VENUEID) ON DELETE NO ACTION,
    CONSTRAINT FK_Bookings_Event FOREIGN KEY (EVENTS_ID) 
        REFERENCES Events(EVENT_ID) ON DELETE NO ACTION,
    
    --- Constraint To Prevent Double Booking ----
    CONSTRAINT UQ_Venue_Event UNIQUE (VENUE_ID, EVENTS_ID),

     CONSTRAINT CK_Booking_Status CHECK (BOOKING_STATUS IN ('Pending', 'Confirmed', 'Cancelled', 'Completed'))
);
GO
--Foreign Key Indexes---
CREATE INDEX IX_Bookings_VenueID ON Bookings(VENUE_ID);
CREATE INDEX IX_Bookings_EventID ON Bookings(EVENTS_ID);
GO

REFERENCING
The EventEase Venue Booking System was developed using Bootstrap 5 as the front-end framework (Bootstrap, 2026). Table styling was implemented using the .table and .table-hover classes as documented in the Bootstrap Tables guide (Bootstrap, 2026). Form controls were styled using .form-control and .form-group classes following Bootstrap's Forms documentation (Bootstrap, 2026). Custom CSS overrides were added to implement gradient backgrounds using the CSS linear-gradient() property (MDN Web Docs, 2026). The layout structure was managed through ASP.NET Core MVC's layout system using _Layout.cshtml as the master page (Microsoft, 2065).
