-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: May 21, 2025 at 01:51 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.1.25

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `onlearndb`
--
CREATE DATABASE IF NOT EXISTS `onlearndb` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `onlearndb`;

DELIMITER $$
--
-- Procedures
--
DROP PROCEDURE IF EXISTS `AddLesson`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddLesson` (IN `p_CourseID` INT, IN `p_LessonTitle` VARCHAR(100), IN `p_LessonContent` TEXT)   BEGIN
    INSERT INTO Lessons (CourseID, LessonTitle, LessonContent)
    VALUES (p_CourseID, p_LessonTitle, p_LessonContent);
END$$

DROP PROCEDURE IF EXISTS `EnrollUser`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `EnrollUser` (IN `p_UserID` INT, IN `p_CourseID` INT)   BEGIN
    INSERT INTO Enrollments (UserID, CourseID)
    VALUES (p_UserID, p_CourseID);
END$$

DROP PROCEDURE IF EXISTS `generateCoursePerformanceReport`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `generateCoursePerformanceReport` (IN `courseID` INT, IN `userID` INT)   BEGIN
    DECLARE totalEnrollments INT;
    DECLARE avgGrade DECIMAL(5,2);

    -- Get total enrollments for the course
    SET totalEnrollments = getCourseEnrollmentDetails(courseID);

    -- Get the average grade of the user in the course
    SET avgGrade = getAverageGrade(userID, courseID);

    -- Display results
    SELECT courseID AS Course_ID, 
           totalEnrollments AS Total_Enrollments, 
           userID AS User_ID, 
           avgGrade AS Average_Grade;
END$$

DROP PROCEDURE IF EXISTS `GetAllUsers`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetAllUsers` ()   BEGIN
    SELECT * FROM Users;
END$$

DROP PROCEDURE IF EXISTS `GetAssignmentSubmissions`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetAssignmentSubmissions` (IN `p_AssignmentID` INT)   BEGIN
    DECLARE done INT DEFAULT FALSE;
    DECLARE v_SubmissionID INT;
    DECLARE v_UserID INT;
    DECLARE v_SubmissionDate DATETIME;
    DECLARE v_Grade DECIMAL(5,2);

    DECLARE submission_cursor CURSOR FOR 
        SELECT SubmissionID, UserID, SubmissionDate, Grade 
        FROM Submissions 
        WHERE AssignmentID = p_AssignmentID;

    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;

    OPEN submission_cursor;

    read_loop: LOOP
        FETCH submission_cursor INTO v_SubmissionID, v_UserID, v_SubmissionDate, v_Grade;
        IF done THEN
            LEAVE read_loop;
        END IF;

        -- Here you can process the fetched data as needed
        -- For example, you could insert it into a temporary table or return it
        SELECT v_SubmissionID AS SubmissionID, v_UserID AS UserID, v_SubmissionDate AS SubmissionDate, v_Grade AS Grade;
    END LOOP;

    CLOSE submission_cursor;
END$$

DROP PROCEDURE IF EXISTS `GetCourseLessons`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetCourseLessons` (IN `p_CourseID` INT)   BEGIN
    DECLARE done INT DEFAULT FALSE;
    DECLARE v_LessonID INT;
    DECLARE v_LessonTitle VARCHAR(100);
    DECLARE v_LessonContent TEXT;

    DECLARE lesson_cursor CURSOR FOR 
        SELECT LessonID, LessonTitle, LessonContent 
        FROM Lessons 
        WHERE CourseID = p_CourseID;

    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;

    OPEN lesson_cursor;

    read_loop: LOOP
        FETCH lesson_cursor INTO v_LessonID, v_LessonTitle, v_LessonContent;
        IF done THEN
            LEAVE read_loop;
        END IF;

        -- Here you can process the fetched data as needed
        SELECT v_LessonID AS LessonID, v_LessonTitle AS LessonTitle, v_LessonContent AS LessonContent;
    END LOOP;

    CLOSE lesson_cursor;
END$$

DROP PROCEDURE IF EXISTS `GetFeedbackForCourse`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetFeedbackForCourse` (IN `p_CourseID` INT)   BEGIN
    DECLARE done INT DEFAULT FALSE;
    DECLARE v_FeedbackID INT;
    DECLARE v_UserID INT;
    DECLARE v_FeedbackText TEXT;
    DECLARE v_FeedbackDate DATETIME;

    DECLARE feedback_cursor CURSOR FOR 
        SELECT FeedbackID, UserID, FeedbackText, FeedbackDate 
        FROM Feedback 
        WHERE CourseID = p_CourseID;

    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;

    OPEN feedback_cursor;

    read_loop: LOOP
        FETCH feedback_cursor INTO v_FeedbackID, v_UserID, v_FeedbackText, v_FeedbackDate;
        IF done THEN
            LEAVE read_loop;
        END IF;

        -- Here you can process the fetched data as needed
        SELECT v_FeedbackID AS FeedbackID, v_UserID AS UserID, v_FeedbackText AS FeedbackText, v_FeedbackDate AS FeedbackDate;
    END LOOP;

    CLOSE feedback_cursor;
END$$

DROP PROCEDURE IF EXISTS `GetUserEnrollments`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetUserEnrollments` (IN `p_UserID` INT)   BEGIN
    DECLARE done INT DEFAULT FALSE;
    DECLARE v_EnrollmentID INT;
    DECLARE v_CourseID INT;
    DECLARE v_EnrollmentDate DATETIME;

    DECLARE enrollment_cursor CURSOR FOR 
        SELECT EnrollmentID, CourseID, EnrollmentDate 
        FROM Enrollments 
        WHERE UserID = p_UserID;

    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;

    OPEN enrollment_cursor;

    read_loop: LOOP
        FETCH enrollment_cursor INTO v_EnrollmentID, v_CourseID, v_EnrollmentDate;
        IF done THEN
            LEAVE read_loop;
        END IF;

        -- Here you can process the fetched data as needed
        SELECT v_EnrollmentID AS EnrollmentID, v_CourseID AS CourseID, v_EnrollmentDate AS EnrollmentDate;
    END LOOP;

    CLOSE enrollment_cursor;
END$$

DROP PROCEDURE IF EXISTS `GetUserPerformance`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetUserPerformance` (IN `user_id` INT)   BEGIN
    SELECT 
        user_id AS UserID,
        GetUserAverageGrade(user_id) AS AverageGrade,
        GetUserTotalSubmissions(user_id) AS TotalSubmissions;
END$$

DROP PROCEDURE IF EXISTS `ListCoursesByUser`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `ListCoursesByUser` (IN `creator_id` INT)   BEGIN
    DECLARE done INT DEFAULT FALSE;
    DECLARE course_id INT;
    DECLARE course_name VARCHAR(100);
    DECLARE course_description TEXT;

    -- Declare the cursor
    DECLARE course_cursor CURSOR FOR 
        SELECT CourseID, CourseName, Description FROM Courses WHERE CreatedBy = creator_id;

    -- Declare the handler for the cursor
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;

    -- Open the cursor
    OPEN course_cursor;

    -- Loop through the cursor
    read_loop: LOOP
        FETCH course_cursor INTO course_id, course_name, course_description;
        IF done THEN
            LEAVE read_loop;
        END IF;

        -- Here you can perform any action with the fetched data
        SELECT CONCAT('Course ID: ', course_id, ', Course Name: ', course_name, ', Description: ', course_description) AS CourseInfo;
    END LOOP;

    -- Close the cursor
    CLOSE course_cursor;
END$$

DROP PROCEDURE IF EXISTS `RegisterUser`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `RegisterUser` (IN `p_Username` VARCHAR(50), IN `p_Password` VARCHAR(255), IN `p_Email` VARCHAR(100), IN `p_UserType` ENUM('Student','Instructor','Admin'))   BEGIN
    INSERT INTO Users (Username, Password, Email, UserType)
    VALUES (p_Username, p_Password, p_Email, p_UserType);
END$$

--
-- Functions
--
DROP FUNCTION IF EXISTS `getAverageGrade`$$
CREATE DEFINER=`root`@`localhost` FUNCTION `getAverageGrade` (`userID` INT, `courseID` INT) RETURNS DECIMAL(5,2) DETERMINISTIC BEGIN
    DECLARE avgGrade DECIMAL(5,2);

    SELECT AVG(s.Grade) INTO avgGrade
    FROM submissions s
    JOIN assignments a ON s.AssignmentID = a.AssignmentID
    WHERE s.UserID = userID AND a.CourseID = courseID;

    RETURN IFNULL(avgGrade, 0);  -- Return 0 if no grades are found
END$$

DROP FUNCTION IF EXISTS `GetCourseCompletionRate`$$
CREATE DEFINER=`root`@`localhost` FUNCTION `GetCourseCompletionRate` (`user_id` INT, `course_id` INT) RETURNS DOUBLE DETERMINISTIC BEGIN
    DECLARE total_assignments INT;
    DECLARE completed_assignments INT;
    DECLARE completion_rate DOUBLE;
    
    -- Get total assignments in the course
    SELECT COUNT(*) INTO total_assignments FROM assignments WHERE CourseID = course_id;
    
    -- Get assignments submitted by the user
    SELECT COUNT(*) INTO completed_assignments 
    FROM submissions s 
    JOIN assignments a ON s.AssignmentID = a.AssignmentID
    WHERE s.UserID = user_id AND a.CourseID = course_id;
    
    -- Calculate completion rate
    IF total_assignments = 0 THEN
        RETURN 0;  -- Avoid division by zero
    ELSE
        SET completion_rate = (completed_assignments / total_assignments) * 100;
        RETURN completion_rate;
    END IF;
END$$

DROP FUNCTION IF EXISTS `getCourseEnrollmentDetails`$$
CREATE DEFINER=`root`@`localhost` FUNCTION `getCourseEnrollmentDetails` (`courseID` INT) RETURNS INT(11) DETERMINISTIC BEGIN
    DECLARE totalEnrollments INT;

    SELECT COUNT(*) INTO totalEnrollments
    FROM enrollments
    WHERE enrollments.CourseID = courseID;

    RETURN totalEnrollments;
END$$

DROP FUNCTION IF EXISTS `GetInstructorName`$$
CREATE DEFINER=`root`@`localhost` FUNCTION `GetInstructorName` (`course_id` INT) RETURNS VARCHAR(100) CHARSET utf8mb4 COLLATE utf8mb4_general_ci DETERMINISTIC BEGIN
    DECLARE instructor_name VARCHAR(100);
    SELECT Username INTO instructor_name FROM users 
    WHERE UserID = (SELECT CreatedBy FROM courses WHERE CourseID = course_id);
    RETURN instructor_name;
END$$

DROP FUNCTION IF EXISTS `GetTotalEnrollments`$$
CREATE DEFINER=`root`@`localhost` FUNCTION `GetTotalEnrollments` (`course_id` INT) RETURNS INT(11) DETERMINISTIC BEGIN
    DECLARE total INT;
    SELECT COUNT(*) INTO total FROM enrollments WHERE CourseID = course_id;
    RETURN total;
END$$

DROP FUNCTION IF EXISTS `GetTotalFeedbackCount`$$
CREATE DEFINER=`root`@`localhost` FUNCTION `GetTotalFeedbackCount` (`course_id` INT) RETURNS INT(11) DETERMINISTIC BEGIN
    DECLARE total_feedback INT;
    
    -- Count the number of feedback entries for the course
    SELECT COUNT(*) INTO total_feedback FROM feedback WHERE CourseID = course_id;
    
    RETURN total_feedback;
END$$

DROP FUNCTION IF EXISTS `getUserGradeDetails`$$
CREATE DEFINER=`root`@`localhost` FUNCTION `getUserGradeDetails` (`userID` INT, `courseID` INT, `assignmentID` INT) RETURNS INT(11) DETERMINISTIC BEGIN
    DECLARE grade INT;

    SELECT s.Grade INTO grade
    FROM submissions s
    JOIN assignments a ON s.AssignmentID = a.AssignmentID
    WHERE s.UserID = userID 
      AND a.CourseID = courseID 
      AND s.AssignmentID = assignmentID
    LIMIT 1;

    RETURN grade;
END$$

DROP FUNCTION IF EXISTS `GetUserGradeStatus`$$
CREATE DEFINER=`root`@`localhost` FUNCTION `GetUserGradeStatus` (`user_id` INT, `course_id` INT) RETURNS VARCHAR(20) CHARSET utf8mb4 COLLATE utf8mb4_general_ci DETERMINISTIC BEGIN
    DECLARE avg_grade DOUBLE;
    DECLARE status VARCHAR(20);
    
    -- Calculate the average grade for the user in a specific course
    SELECT AVG(s.Grade) INTO avg_grade
    FROM submissions s
    JOIN assignments a ON s.AssignmentID = a.AssignmentID
    WHERE s.UserID = user_id AND a.CourseID = course_id;
    
    -- If no grades exist, set avg_grade to 0
    IF avg_grade IS NULL THEN
        SET avg_grade = 0;
    END IF;
    
    -- Determine pass/fail status (assuming passing grade is 75)
    IF avg_grade >= 75 THEN
        SET status = 'PASSED';
    ELSE
        SET status = 'FAILED';
    END IF;
    
    RETURN status;
END$$

DROP FUNCTION IF EXISTS `GetUserTotalSubmissions`$$
CREATE DEFINER=`root`@`localhost` FUNCTION `GetUserTotalSubmissions` (`user_id` INT) RETURNS INT(11) DETERMINISTIC BEGIN
    DECLARE total_submissions INT;
    SELECT COUNT(*) INTO total_submissions FROM submissions WHERE UserID = user_id;
    RETURN total_submissions;
END$$

DROP FUNCTION IF EXISTS `GetUserType`$$
CREATE DEFINER=`root`@`localhost` FUNCTION `GetUserType` (`user_id` INT) RETURNS CHAR(20) CHARSET utf8mb4 COLLATE utf8mb4_general_ci DETERMINISTIC BEGIN
    DECLARE user_type CHAR(20);
    SELECT UserType INTO user_type FROM users WHERE UserID = user_id;
    RETURN user_type;
END$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Stand-in structure for view `admin_system_overview`
-- (See below for the actual view)
--
DROP VIEW IF EXISTS `admin_system_overview`;
CREATE TABLE `admin_system_overview` (
`Total_Active_Users` bigint(21)
,`New_Enrollments_This_Week` bigint(21)
,`Courses_Without_Assignments` bigint(21)
,`Avg_Feedback_Rating` decimal(14,4)
,`Ungraded_Submissions_Pct` decimal(25,1)
,`Recent_Audit_Entry` varchar(104)
,`Inactive_Instructors` bigint(21)
,`High_Enrollment_Courses` mediumtext
,`Password_Reset_Requests` bigint(21)
,`System_Storage_Usage_Estimate` bigint(21)
);

-- --------------------------------------------------------

--
-- Table structure for table `assignments`
--

DROP TABLE IF EXISTS `assignments`;
CREATE TABLE `assignments` (
  `AssignmentID` int(11) NOT NULL,
  `CourseID` int(11) NOT NULL,
  `AssignmentTitle` varchar(100) NOT NULL,
  `DueDate` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `assignments`
--

INSERT INTO `assignments` (`AssignmentID`, `CourseID`, `AssignmentTitle`, `DueDate`) VALUES
(1, 1, 'Updated Title', '2023-10-15 23:59:59'),
(2, 1, 'Geometry Project', '2023-10-22 23:59:59'),
(3, 2, 'Physics Lab Report', '2023-10-20 23:59:59'),
(4, 3, 'Chemistry Experiment', '2023-10-25 23:59:59'),
(5, 4, 'Biology Research Paper', '2023-10-30 23:59:59'),
(6, 5, 'History Essay', '2023-11-05 23:59:59'),
(7, 6, 'CS Programming Assignment', '2023-11-10 23:59:59'),
(8, 7, 'Art Project', '2023-11-15 23:59:59'),
(9, 8, 'Kanji and Hiragana Research Review ', '2025-02-19 13:47:52'),
(10, 10, 'Experiment 1 - Analysis', '2025-02-19 13:57:23');

--
-- Triggers `assignments`
--
DROP TRIGGER IF EXISTS `after_assignment_update`;
DELIMITER $$
CREATE TRIGGER `after_assignment_update` AFTER UPDATE ON `assignments` FOR EACH ROW BEGIN
    INSERT INTO audit_log (action, table_name, record_id, timestamp)
    VALUES ('UPDATE', 'assignments', NEW.AssignmentID, NOW());
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `audit_log`
--

DROP TABLE IF EXISTS `audit_log`;
CREATE TABLE `audit_log` (
  `log_id` int(11) NOT NULL,
  `action` varchar(50) DEFAULT NULL,
  `table_name` varchar(50) DEFAULT NULL,
  `record_id` int(11) DEFAULT NULL,
  `timestamp` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `audit_log`
--

INSERT INTO `audit_log` (`log_id`, `action`, `table_name`, `record_id`, `timestamp`) VALUES
(2, 'INSERT', 'users', 14, '2025-03-23 21:01:51'),
(3, 'INSERT ATTEMPT', 'courses', 11, '2025-03-23 21:11:41'),
(4, 'UPDATE', 'assignments', 1, '2025-03-23 21:16:07'),
(5, 'LESSON UPDATE ATTEMPT', 'lessons', 1, '2025-03-23 21:35:20'),
(6, 'DELETE', 'submissions', 6, '2025-03-23 21:38:21'),
(7, 'DELETE CONFIRMATION', 'feedback', 2, '2025-03-23 21:39:16'),
(8, 'INSERT', 'users', 15, '2025-03-23 21:43:18'),
(9, 'INSERT ATTEMPT', 'courses', 12, '2025-03-23 21:45:37'),
(10, 'INSERT ATTEMPT', 'courses', 13, '2025-03-23 21:46:39'),
(11, 'LESSON UPDATE ATTEMPT', 'lessons', 2, '2025-03-23 21:50:28'),
(12, 'DELETE', 'submissions', 9, '2025-03-23 21:52:14'),
(13, 'DELETE CONFIRMATION', 'feedback', 8, '2025-03-23 21:53:29'),
(14, 'UPDATE', 'users', 3, '2025-05-21 11:03:39'),
(15, 'PASSWORD_UPDATE', 'users', 1, '2025-05-21 11:13:19'),
(16, 'INSERT', 'users', 16, '2025-05-21 11:40:20'),
(17, 'PASSWORD_UPDATE', 'users', 1, '2025-05-21 19:18:55');

-- --------------------------------------------------------

--
-- Table structure for table `courses`
--

DROP TABLE IF EXISTS `courses`;
CREATE TABLE `courses` (
  `CourseID` int(11) NOT NULL,
  `CourseName` varchar(100) NOT NULL,
  `Description` text DEFAULT NULL,
  `CreatedBy` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `courses`
--

INSERT INTO `courses` (`CourseID`, `CourseName`, `Description`, `CreatedBy`) VALUES
(1, 'Mathematics 101', 'Introduction to Mathematics', 3),
(2, 'Physics 101', 'Introduction to Physics', 3),
(3, 'Chemistry 101', 'Introduction to Chemistry', 3),
(4, 'Biology 101', 'Introduction to Biology', 3),
(5, 'History 101', 'Introduction to History', 3),
(6, 'Computer Science 101', 'Introduction to Computer Science', 7),
(7, 'Art 101', 'Introduction to Art', 7),
(8, 'Japanese Language 101', 'Learning the Japanese alphabets', 7),
(9, 'Mathematics 102', 'History of Numbers', 7),
(10, 'Biology 102', 'Basic Life on Earth', 7),
(11, 'Database Management', 'A course on databases', 13),
(12, 'Chinese Language 101', 'An Introduction to chinese scripts', 13);

--
-- Triggers `courses`
--
DROP TRIGGER IF EXISTS `before_course_insert`;
DELIMITER $$
CREATE TRIGGER `before_course_insert` BEFORE INSERT ON `courses` FOR EACH ROW BEGIN
    IF NEW.CourseName IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Course Name cannot be NULL';
    ELSE
        INSERT INTO audit_log (action, table_name, record_id, timestamp)
        VALUES ('INSERT ATTEMPT', 'courses', NEW.CourseID, NOW());
    END IF;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Stand-in structure for view `coursewithinstructor`
-- (See below for the actual view)
--
DROP VIEW IF EXISTS `coursewithinstructor`;
CREATE TABLE `coursewithinstructor` (
`CourseID` int(11)
,`CourseName` varchar(100)
,`Instructor` varchar(100)
);

-- --------------------------------------------------------

--
-- Table structure for table `enrollments`
--

DROP TABLE IF EXISTS `enrollments`;
CREATE TABLE `enrollments` (
  `EnrollmentID` int(11) NOT NULL,
  `UserID` int(11) NOT NULL,
  `CourseID` int(11) NOT NULL,
  `EnrollmentDate` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `enrollments`
--

INSERT INTO `enrollments` (`EnrollmentID`, `UserID`, `CourseID`, `EnrollmentDate`) VALUES
(1, 1, 1, '2025-02-01 22:24:41'),
(2, 1, 2, '2025-02-01 22:24:41'),
(3, 2, 1, '2025-02-01 22:24:41'),
(4, 2, 3, '2025-02-01 22:24:41'),
(5, 5, 4, '2025-02-01 22:24:41'),
(6, 5, 5, '2025-02-01 22:24:41'),
(7, 5, 6, '2025-02-01 22:24:41'),
(8, 6, 7, '2025-02-01 22:24:41'),
(9, 1, 6, '2025-02-01 22:24:41'),
(10, 2, 7, '2025-02-01 22:24:41'),
(11, 12, 1, '2025-02-20 20:01:29'),
(12, 16, 4, '2025-05-21 11:40:20');

-- --------------------------------------------------------

--
-- Table structure for table `feedback`
--

DROP TABLE IF EXISTS `feedback`;
CREATE TABLE `feedback` (
  `FeedbackID` int(11) NOT NULL,
  `CourseID` int(11) NOT NULL,
  `UserID` int(11) NOT NULL,
  `FeedbackText` text NOT NULL,
  `FeedbackDate` datetime DEFAULT current_timestamp(),
  `Rating` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `feedback`
--

INSERT INTO `feedback` (`FeedbackID`, `CourseID`, `UserID`, `FeedbackText`, `FeedbackDate`, `Rating`) VALUES
(3, 2, 3, 'The lab sessions were very helpful.', '2025-02-01 22:31:33', 5),
(4, 3, 4, 'I learned a lot about chemical reactions.', '2025-02-01 22:31:33', 4),
(5, 4, 5, 'The biology course was engaging.', '2025-02-01 22:31:33', 4),
(6, 5, 6, 'History lessons were well structured.', '2025-02-01 22:31:33', 5),
(7, 6, 1, 'Computer Science is challenging but fun.', '2025-02-01 22:31:33', 4);

--
-- Triggers `feedback`
--
DROP TRIGGER IF EXISTS `before_feedback_delete`;
DELIMITER $$
CREATE TRIGGER `before_feedback_delete` BEFORE DELETE ON `feedback` FOR EACH ROW BEGIN
    DECLARE confirm_delete INT;
    SET confirm_delete = (SELECT COUNT(*) FROM feedback WHERE FeedbackID = OLD.FeedbackID);
    
    IF confirm_delete > 0 THEN
        INSERT INTO audit_log (action, table_name, record_id, timestamp)
        VALUES ('DELETE CONFIRMATION', 'feedback', OLD.FeedbackID, NOW());
    END IF;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Stand-in structure for view `instructor_course_analytics`
-- (See below for the actual view)
--
DROP VIEW IF EXISTS `instructor_course_analytics`;
CREATE TABLE `instructor_course_analytics` (
`CourseID` int(11)
,`CourseName` varchar(100)
,`Enrolled_Students` bigint(21)
,`Active_Assignments` bigint(21)
,`Ungraded_Submissions` bigint(21)
,`Avg_Feedback_Rating` decimal(14,4)
,`Recent_Feedback` mediumtext
,`Next_Assignment_Due` datetime
,`Quiz_Count` bigint(21)
,`Course_Description` text
,`Submission_Rate_Pct` decimal(25,1)
);

-- --------------------------------------------------------

--
-- Stand-in structure for view `instructor_course_overview`
-- (See below for the actual view)
--
DROP VIEW IF EXISTS `instructor_course_overview`;
CREATE TABLE `instructor_course_overview` (
`CourseID` int(11)
,`CourseName` varchar(100)
,`Enrolled_Students` bigint(21)
,`Active_Assignments` bigint(21)
,`Ungraded_Submissions` bigint(21)
,`Avg_Feedback_Rating` decimal(12,1)
,`Recent_Feedback` varchar(50)
,`Next_Assignment_Due` datetime
,`Quiz_Count` bigint(21)
,`Course_Description` text
,`Submission_Rate_Pct` decimal(25,1)
);

-- --------------------------------------------------------

--
-- Table structure for table `lessons`
--

DROP TABLE IF EXISTS `lessons`;
CREATE TABLE `lessons` (
  `LessonID` int(11) NOT NULL,
  `CourseID` int(11) NOT NULL,
  `LessonTitle` varchar(100) NOT NULL,
  `LessonContent` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `lessons`
--

INSERT INTO `lessons` (`LessonID`, `CourseID`, `LessonTitle`, `LessonContent`) VALUES
(1, 1, 'Database Fundamentals', 'Introduction to databases'),
(2, 1, 'Algebra Basics', 'Introduction to Algebra'),
(3, 2, 'Newtonian Mechanics', 'Fundamentals of motion and forces.'),
(4, 2, 'Thermodynamics', 'Basic principles of heat and energy.'),
(5, 3, 'Atomic Structure', 'Understanding the building blocks of matter.'),
(6, 3, 'Chemical Reactions', 'Introduction to different types of chemical reactions.'),
(7, 4, 'Cell Biology', 'Basics of cell structure and function.'),
(8, 4, 'Genetics', 'Introduction to heredity and genetic variation.'),
(9, 5, 'Ancient Civilizations', 'Overview of early human societies.'),
(10, 5, 'Modern History', 'Key events in modern history.'),
(11, 1, 'Geometry Basics 2', 'Exploring 2D Shapes: Circles, Squares, and Triangles');

--
-- Triggers `lessons`
--
DROP TRIGGER IF EXISTS `before_lesson_update`;
DELIMITER $$
CREATE TRIGGER `before_lesson_update` BEFORE UPDATE ON `lessons` FOR EACH ROW BEGIN
    IF NEW.LessonTitle IS NULL OR NEW.LessonTitle = '' OR NEW.LessonTitle REGEXP '^[[:space:]]*$' 
       OR NEW.LessonContent IS NULL OR NEW.LessonContent = '' OR NEW.LessonContent REGEXP '^[[:space:]]*$' THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Lesson title and content cannot be empty or contain only whitespace';
    ELSE
        INSERT INTO audit_log (log_id, action, table_name, record_id, timestamp)
        VALUES (NULL, 'LESSON UPDATE ATTEMPT', 'lessons', NEW.LessonID, NOW());
    END IF;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Stand-in structure for view `numberofcoursecreatedbyinstructor`
-- (See below for the actual view)
--
DROP VIEW IF EXISTS `numberofcoursecreatedbyinstructor`;
CREATE TABLE `numberofcoursecreatedbyinstructor` (
`UserID` int(11)
,`InstructorName` varchar(50)
,`NumberOfCourses` bigint(21)
);

-- --------------------------------------------------------

--
-- Table structure for table `quizzes`
--

DROP TABLE IF EXISTS `quizzes`;
CREATE TABLE `quizzes` (
  `quiz_id` int(11) NOT NULL,
  `courseid` int(11) NOT NULL,
  `title` varchar(255) NOT NULL,
  `description` text DEFAULT NULL,
  `total_marks` int(11) NOT NULL,
  `created_at` timestamp NOT NULL DEFAULT current_timestamp(),
  `updated_at` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `quizzes`
--

INSERT INTO `quizzes` (`quiz_id`, `courseid`, `title`, `description`, `total_marks`, `created_at`, `updated_at`) VALUES
(1, 1, 'Numbers & Logic Challenge', 'Test your mathematical reasoning with a mix of algebra, geometry, and number theory.', 100, '2025-02-27 15:02:57', '2025-02-27 15:02:57'),
(2, 1, 'Mathematical Mind Marathon', 'A challenging set of problems to stretch your problem-solving skills!', 100, '2025-02-27 15:02:57', '2025-02-27 15:02:57'),
(3, 2, 'Forces & Motion Mastery', 'Assess your understanding of Newton’s Laws and kinematics.', 100, '2025-02-27 15:02:57', '2025-02-27 15:02:57'),
(4, 2, 'Quantum Quandaries', 'Dive into the mysteries of modern physics with this mind-bending quiz.', 100, '2025-02-27 15:02:57', '2025-02-27 15:02:57'),
(5, 3, 'Elements & Compounds Quiz', 'Explore the periodic table and chemical reactions in this engaging test.', 100, '2025-02-27 15:02:57', '2025-02-27 15:02:57'),
(6, 3, 'Chemical Bonds & Beyond', 'Test your knowledge on atomic structures, bonding, and stoichiometry.', 100, '2025-02-27 15:02:57', '2025-02-27 15:02:57'),
(7, 4, 'Cell Biology Brain Teaser', 'Challenge yourself with questions on cellular structures and functions.', 100, '2025-02-27 15:02:57', '2025-02-27 15:02:57'),
(8, 4, 'Genetics & Evolution Trivia', 'A fun way to assess your grasp on heredity, DNA, and evolutionary theories.', 100, '2025-02-27 15:02:57', '2025-02-27 15:02:57'),
(9, 5, 'Ancient Civilizations Quest', 'How well do you know the great empires and revolutions of the past?', 100, '2025-02-27 15:02:57', '2025-02-27 15:02:57'),
(10, 5, 'Modern History Challenge', 'Explore the major historical events from the 19th century to today.', 100, '2025-02-27 15:02:57', '2025-02-27 15:02:57'),
(11, 6, 'Code & Logic Blitz', 'A quiz on fundamental programming concepts and logic puzzles.', 100, '2025-02-27 15:02:57', '2025-02-27 15:02:57'),
(12, 6, 'Cybersecurity & Networking Basics', 'Test your understanding of online security and networking principles.', 100, '2025-02-27 15:02:57', '2025-02-27 15:02:57'),
(13, 7, 'Art Movements & Masterpieces', 'Identify famous artists and their iconic works.', 100, '2025-02-27 15:02:57', '2025-02-27 15:02:57'),
(14, 7, 'Color Theory & Design Quiz', 'Understand the science and emotion behind colors in art.', 100, '2025-02-27 15:02:57', '2025-02-27 15:02:57'),
(15, 8, 'Hiragana & Katakana Basics', 'Master the Japanese writing systems in this quiz.', 100, '2025-02-27 15:02:57', '2025-02-27 15:02:57'),
(16, 8, 'Grammar & Vocabulary Test', 'A challenge covering Japanese sentence structures and essential phrases.', 100, '2025-02-27 15:02:57', '2025-02-27 15:02:57'),
(17, 9, 'Advanced Algebra & Calculus', 'Evaluate your skills in derivatives, integrals, and algebraic proofs.', 100, '2025-02-27 15:02:57', '2025-02-27 15:02:57'),
(18, 9, 'Applied Mathematics & Problem Solving', 'Solve real-world problems with mathematical models.', 100, '2025-02-27 15:02:57', '2025-02-27 15:02:57'),
(19, 10, 'Human Anatomy & Physiology', 'Explore the systems of the human body and how they function.', 100, '2025-02-27 15:02:57', '2025-02-27 15:02:57'),
(20, 10, 'Ecology & Environmental Science', 'Assess your knowledge of ecosystems, biodiversity, and conservation.', 100, '2025-02-27 15:02:57', '2025-02-27 15:02:57');

-- --------------------------------------------------------

--
-- Table structure for table `quiz_answers`
--

DROP TABLE IF EXISTS `quiz_answers`;
CREATE TABLE `quiz_answers` (
  `answer_id` int(20) UNSIGNED NOT NULL,
  `question_id` int(11) NOT NULL,
  `answer_text` text NOT NULL,
  `is_correct` tinyint(1) NOT NULL,
  `created_at` timestamp NOT NULL DEFAULT current_timestamp(),
  `updated_at` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `quiz_answers`
--

INSERT INTO `quiz_answers` (`answer_id`, `question_id`, `answer_text`, `is_correct`, `created_at`, `updated_at`) VALUES
(1, 1, '3.142', 1, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(2, 1, '3.141', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(3, 1, '3.15', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(4, 1, '3.14', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(5, 2, 'x = 3', 1, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(6, 2, 'x = 5', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(7, 2, 'x = 2', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(8, 2, 'x = 1', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(9, 3, '12', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(10, 3, '14', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(11, 3, '144', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(12, 3, '12', 1, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(13, 4, '6', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(14, 4, '7', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(15, 4, '8', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(16, 4, '8', 1, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(17, 5, '90°', 1, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(18, 5, '45°', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(19, 5, '120°', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(20, 5, '75°', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(21, 6, '13', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(22, 6, '11', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(23, 6, '10', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(24, 6, '13', 1, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(25, 7, '2x', 1, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(26, 7, 'x', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(27, 7, 'x²', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(28, 7, '4x', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(29, 8, '4', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(30, 8, '9', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(31, 8, '11', 1, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(32, 8, '15', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(33, 9, '540°', 1, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(34, 9, '360°', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(35, 9, '720°', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(36, 9, '600°', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(37, 10, '120', 1, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(38, 10, '60', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(39, 10, '24', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(40, 10, '150', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(41, 11, 'Newton’s First Law', 1, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(42, 11, 'Newton’s Second Law', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(43, 11, 'Newton’s Third Law', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(44, 11, 'Law of Gravity', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(45, 12, 'F = ma', 1, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(46, 12, 'F = mv', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(47, 12, 'F = m/a', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(48, 12, 'F = m+v', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(49, 13, '20 m/s²', 1, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(50, 13, '15 m/s²', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(51, 13, '30 m/s²', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(52, 13, '25 m/s²', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(53, 14, 'Newton', 1, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(54, 14, 'Joule', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(55, 14, 'Watt', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(56, 14, 'Pascal', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(57, 15, 'Gravity', 1, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(58, 15, 'Friction', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(59, 15, 'Electromagnetic force', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(60, 15, 'Nuclear force', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(61, 16, 'Albert Einstein', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(62, 16, 'Niels Bohr', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(63, 16, 'Erwin Schrödinger', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(64, 16, 'Max Planck', 1, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(65, 17, 'It states that energy is always conserved', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(66, 17, 'It states that particles can exist in multiple places at once', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(67, 17, 'It states that we cannot precisely measure both the position and momentum of a particle simultaneously', 1, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(68, 17, 'It states that electrons orbit in fixed paths', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(69, 18, 'Photon', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(70, 18, 'Gluon', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(71, 18, 'Electron', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(72, 18, 'Photon', 1, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(73, 19, 'It emits light', 1, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(74, 19, 'It loses mass', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(75, 19, 'It disappears', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(76, 19, 'It gains energy', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(77, 20, 'The randomness of quantum mechanics', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(78, 20, 'The paradox of superposition and observation in quantum mechanics', 1, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(79, 20, 'The nature of black holes', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22'),
(80, 20, 'The structure of atoms', 0, '2025-02-27 15:22:22', '2025-02-27 15:22:22');

-- --------------------------------------------------------

--
-- Table structure for table `quiz_questions`
--

DROP TABLE IF EXISTS `quiz_questions`;
CREATE TABLE `quiz_questions` (
  `question_id` int(11) NOT NULL,
  `quiz_id` int(11) NOT NULL,
  `question_text` text NOT NULL,
  `question_type` varchar(50) NOT NULL,
  `created_at` timestamp NOT NULL DEFAULT current_timestamp(),
  `updated_at` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `quiz_questions`
--

INSERT INTO `quiz_questions` (`question_id`, `quiz_id`, `question_text`, `question_type`, `created_at`, `updated_at`) VALUES
(1, 1, 'What is the value of π (pi) to three decimal places?', 'multiple choice', '2025-02-27 15:20:14', '2025-02-27 15:20:14'),
(2, 1, 'Solve for x: 5x + 3 = 18.', 'multiple choice', '2025-02-27 15:20:14', '2025-02-27 15:20:14'),
(3, 1, 'What is the square root of 144?', 'multiple choice', '2025-02-27 15:20:14', '2025-02-27 15:20:14'),
(4, 1, 'Which sequence follows the Fibonacci pattern: 1, 1, 2, 3, 5, ?', 'multiple choice', '2025-02-27 15:20:14', '2025-02-27 15:20:14'),
(5, 1, 'If a triangle has angles 30° and 60°, what is the third angle?', 'multiple choice', '2025-02-27 15:20:14', '2025-02-27 15:20:14'),
(6, 2, 'If f(x) = 2x + 3, what is f(5)?', 'multiple choice', '2025-02-27 15:20:14', '2025-02-27 15:20:14'),
(7, 2, 'What is the derivative of x²?', 'multiple choice', '2025-02-27 15:20:14', '2025-02-27 15:20:14'),
(8, 2, 'Which of the following is a prime number: 4, 9, 11, 15?', 'multiple choice', '2025-02-27 15:20:14', '2025-02-27 15:20:14'),
(9, 2, 'What is the sum of the interior angles of a hexagon?', 'multiple choice', '2025-02-27 15:20:14', '2025-02-27 15:20:14'),
(10, 2, 'What is the factorial of 5?', 'multiple choice', '2025-02-27 15:20:14', '2025-02-27 15:20:14'),
(11, 3, 'Which law states that an object at rest stays at rest unless acted upon by an external force?', 'multiple choice', '2025-02-27 15:20:14', '2025-02-27 15:20:14'),
(12, 3, 'What is the formula for calculating force?', 'multiple choice', '2025-02-27 15:20:14', '2025-02-27 15:20:14'),
(13, 3, 'If a car accelerates from 0 to 60 m/s in 3 seconds, what is its acceleration?', 'multiple choice', '2025-02-27 15:20:14', '2025-02-27 15:20:14'),
(14, 3, 'What is the SI unit of force?', 'multiple choice', '2025-02-27 15:20:14', '2025-02-27 15:20:14'),
(15, 3, 'Which force keeps planets in orbit around the sun?', 'multiple choice', '2025-02-27 15:20:14', '2025-02-27 15:20:14'),
(16, 4, 'Who is considered the father of quantum mechanics?', 'multiple choice', '2025-02-27 15:20:14', '2025-02-27 15:20:14'),
(17, 4, 'What is the Heisenberg Uncertainty Principle?', 'multiple choice', '2025-02-27 15:20:14', '2025-02-27 15:20:14'),
(18, 4, 'Which particle is responsible for carrying the electromagnetic force?', 'multiple choice', '2025-02-27 15:20:14', '2025-02-27 15:20:14'),
(19, 4, 'What happens when an electron moves from a higher to a lower energy level?', 'multiple choice', '2025-02-27 15:20:14', '2025-02-27 15:20:14'),
(20, 4, 'What is Schrödinger’s cat thought experiment meant to illustrate?', 'multiple choice', '2025-02-27 15:20:14', '2025-02-27 15:20:14');

-- --------------------------------------------------------

--
-- Stand-in structure for view `student_dashboard`
-- (See below for the actual view)
--
DROP VIEW IF EXISTS `student_dashboard`;
CREATE TABLE `student_dashboard` (
`UserID` int(11)
,`CourseID` int(11)
,`CourseName` varchar(100)
,`CourseDescription` text
,`InstructorName` varchar(50)
,`InstructorEmail` varchar(100)
,`EnrollmentDate` datetime
,`NextAssignment` varchar(100)
,`NextAssignmentDueDate` datetime
,`RecentGrade` decimal(5,2)
,`LastSubmissionDate` datetime
,`NextQuiz` varchar(255)
,`QuizTotalMarks` int(11)
,`QuizAvailableFrom` timestamp
,`FeedbackRating` int(11)
,`LastFeedbackDate` datetime
);

-- --------------------------------------------------------

--
-- Stand-in structure for view `student_performance_summary`
-- (See below for the actual view)
--
DROP VIEW IF EXISTS `student_performance_summary`;
CREATE TABLE `student_performance_summary` (
`UserID` int(11)
,`Student_Name` varchar(50)
,`Courses_Taken` bigint(21)
,`Avg_Grade` decimal(9,6)
,`Assignments_Missed` bigint(21)
,`Feedback_Responsiveness` decimal(25,1)
,`Total_Quiz_Questions` bigint(21)
,`Best_Performing_Course` varchar(100)
,`Last_Active_Date` datetime
,`Alert_Status` varchar(19)
);

-- --------------------------------------------------------

--
-- Table structure for table `submissions`
--

DROP TABLE IF EXISTS `submissions`;
CREATE TABLE `submissions` (
  `SubmissionID` int(11) NOT NULL,
  `AssignmentID` int(11) NOT NULL,
  `UserID` int(11) NOT NULL,
  `SubmissionDate` datetime DEFAULT current_timestamp(),
  `Grade` decimal(5,2) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `submissions`
--

INSERT INTO `submissions` (`SubmissionID`, `AssignmentID`, `UserID`, `SubmissionDate`, `Grade`) VALUES
(1, 1, 1, '2023-10-14 10:00:00', 85.00),
(2, 1, 2, '2023-10-15 09:00:00', 90.00),
(3, 2, 1, '2023-10-21 12:00:00', 80.00),
(4, 3, 3, '2023-10-19 15:00:00', 88.00),
(5, 4, 2, '2023-10-24 11:00:00', 92.00),
(7, 6, 5, '2023-11-09 16:00:00', 95.00),
(8, 7, 6, '2023-11-14 18:00:00', 87.00),
(10, 6, 6, '2025-02-19 20:23:04', 89.91);

--
-- Triggers `submissions`
--
DROP TRIGGER IF EXISTS `after_submission_delete`;
DELIMITER $$
CREATE TRIGGER `after_submission_delete` AFTER DELETE ON `submissions` FOR EACH ROW BEGIN
    INSERT INTO audit_log (action, table_name, record_id, timestamp)
    VALUES ('DELETE', 'submissions', OLD.SubmissionID, NOW());
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Stand-in structure for view `totalenrollmentspercourse`
-- (See below for the actual view)
--
DROP VIEW IF EXISTS `totalenrollmentspercourse`;
CREATE TABLE `totalenrollmentspercourse` (
`CourseID` int(11)
,`CourseName` varchar(100)
,`TotalEnrollments` bigint(21)
);

-- --------------------------------------------------------

--
-- Stand-in structure for view `userassignmengrades`
-- (See below for the actual view)
--
DROP VIEW IF EXISTS `userassignmengrades`;
CREATE TABLE `userassignmengrades` (
`UserID` int(11)
,`Username` varchar(50)
,`CourseName` varchar(100)
,`AssignmentTitle` varchar(100)
,`Grade` decimal(5,2)
,`STATUS` varchar(4)
);

-- --------------------------------------------------------

--
-- Stand-in structure for view `userassignmentdetails`
-- (See below for the actual view)
--
DROP VIEW IF EXISTS `userassignmentdetails`;
CREATE TABLE `userassignmentdetails` (
`UserID` int(11)
,`Username` varchar(50)
,`CourseName` varchar(100)
,`AssignmentTitle` varchar(100)
,`Grade` int(11)
,`Status` varchar(9)
,`Mark` varchar(6)
);

-- --------------------------------------------------------

--
-- Stand-in structure for view `userenrollmentswithinstructor`
-- (See below for the actual view)
--
DROP VIEW IF EXISTS `userenrollmentswithinstructor`;
CREATE TABLE `userenrollmentswithinstructor` (
`EnrollmentID` int(11)
,`UserID` int(11)
,`Username` varchar(50)
,`CourseID` int(11)
,`CourseName` varchar(100)
,`InstructorName` varchar(50)
,`EnrollmentDate` datetime
);

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
  `UserID` int(11) NOT NULL,
  `Username` varchar(50) NOT NULL,
  `Password` varchar(255) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `UserType` enum('Student','Instructor','Admin') NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`UserID`, `Username`, `Password`, `Email`, `UserType`) VALUES
(1, 'Sarah Lahbati', 'password01', 'slahbati@example.com', 'Student'),
(2, 'Richard Gomez', 'password2', 'rgomez@example.com', 'Student'),
(3, 'Kyline Alcantara', 'password03', 'kalacantara@example.com', 'Instructor'),
(4, 'Radson Hipolito', 'password4', 'admin1@example.com', 'Admin'),
(5, 'Raymart Santiago', 'password5', 'rsantiago@example.com', 'Student'),
(6, 'Daniel Padilla', 'password6', 'dpadilla@example.com', 'Student'),
(7, 'Ricardo Dalisay', 'password7', 'rdalisay@example.com', 'Instructor'),
(8, 'Michael Lonceras', 'password8', 'admin2@example.com', 'Admin'),
(9, 'Barbie Forteza', 'password9', 'bforteza@example.com', 'Student'),
(10, 'Jackie Chan', 'password10', 'jchan@example.com', 'Student'),
(12, 'John Doe', 'securePassword123', 'john.doe@example.com', 'Student'),
(13, 'Padre Salvi', 'passwordnisalvi', 'psalvi@example.com', 'Instructor'),
(14, 'Keanu Reeves', 'passwordnikeanu', 'keanu@gmail.com', 'Student'),
(15, 'Manny Pacquiao', 'mannyPACQ', 'mannypacq@gmail.com', 'Admin'),
(16, 'Mika Salamanca', '', '', 'Student');

--
-- Triggers `users`
--
DROP TRIGGER IF EXISTS `UserPasswordChange`;
DELIMITER $$
CREATE TRIGGER `UserPasswordChange` AFTER UPDATE ON `users` FOR EACH ROW BEGIN
    -- Log whenever Password is included in UPDATE, regardless of value change
    IF NEW.Password IS NOT NULL THEN
        INSERT INTO audit_log (action, table_name, record_id, timestamp)
        VALUES (
            'PASSWORD_UPDATE',
            'users',
            NEW.UserID,
            NOW()
        );
    END IF;
END
$$
DELIMITER ;
DROP TRIGGER IF EXISTS `after_user_insert`;
DELIMITER $$
CREATE TRIGGER `after_user_insert` AFTER INSERT ON `users` FOR EACH ROW BEGIN
    INSERT INTO audit_log (action, table_name, record_id, timestamp)
    VALUES ('INSERT', 'users', NEW.UserID, NOW());
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Structure for view `admin_system_overview`
--
DROP TABLE IF EXISTS `admin_system_overview`;

DROP VIEW IF EXISTS `admin_system_overview`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `admin_system_overview`  AS SELECT (select count(0) from `users`) AS `Total_Active_Users`, (select count(0) from `enrollments` where `enrollments`.`EnrollmentDate` >= current_timestamp() - interval 7 day) AS `New_Enrollments_This_Week`, (select count(0) from (`courses` `c` left join `assignments` `a` on(`c`.`CourseID` = `a`.`CourseID`)) where `a`.`AssignmentID` is null) AS `Courses_Without_Assignments`, (select avg(`feedback`.`Rating`) from `feedback`) AS `Avg_Feedback_Rating`, (select round(count(case when `submissions`.`Grade` is null then 1 end) * 100.0 / count(0),1) from `submissions`) AS `Ungraded_Submissions_Pct`, (select concat(`audit_log`.`action`,' on ',`audit_log`.`table_name`) from `audit_log` order by `audit_log`.`timestamp` desc limit 1) AS `Recent_Audit_Entry`, (select count(distinct `u`.`UserID`) from `users` `u` where `u`.`UserType` = 'Instructor' and !exists(select 1 from `courses` `c` where `c`.`CreatedBy` = `u`.`UserID` and `c`.`CourseID` in (select `assignments`.`CourseID` from `assignments` where `assignments`.`DueDate` >= current_timestamp() - interval 30 day) limit 1)) AS `Inactive_Instructors`, (select group_concat(`top_courses`.`CourseName` separator ', ') from (select `c`.`CourseName` AS `CourseName` from (`courses` `c` join `enrollments` `e` on(`c`.`CourseID` = `e`.`CourseID`)) group by `c`.`CourseID` order by count(0) desc limit 3) `top_courses`) AS `High_Enrollment_Courses`, (select count(0) from `audit_log` where `audit_log`.`action` like 'password_reset%' and `audit_log`.`timestamp` >= current_timestamp() - interval 7 day) AS `Password_Reset_Requests`, (select count(0) from `submissions` where `submissions`.`SubmissionDate` is not null) AS `System_Storage_Usage_Estimate` ;

-- --------------------------------------------------------

--
-- Structure for view `coursewithinstructor`
--
DROP TABLE IF EXISTS `coursewithinstructor`;

DROP VIEW IF EXISTS `coursewithinstructor`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `coursewithinstructor`  AS SELECT `c`.`CourseID` AS `CourseID`, `c`.`CourseName` AS `CourseName`, `GetInstructorName`(`c`.`CourseID`) AS `Instructor` FROM `courses` AS `c` ;

-- --------------------------------------------------------

--
-- Structure for view `instructor_course_analytics`
--
DROP TABLE IF EXISTS `instructor_course_analytics`;

DROP VIEW IF EXISTS `instructor_course_analytics`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `instructor_course_analytics`  AS SELECT `c`.`CourseID` AS `CourseID`, `c`.`CourseName` AS `CourseName`, (select count(0) from `enrollments` `e` where `e`.`CourseID` = `c`.`CourseID`) AS `Enrolled_Students`, (select count(0) from `assignments` `a` where `a`.`CourseID` = `c`.`CourseID` and `a`.`DueDate` >= current_timestamp()) AS `Active_Assignments`, (select count(0) from (`assignments` `a` join `submissions` `s` on(`a`.`AssignmentID` = `s`.`AssignmentID`)) where `a`.`CourseID` = `c`.`CourseID` and `s`.`Grade` is null) AS `Ungraded_Submissions`, (select avg(`f`.`Rating`) from `feedback` `f` where `f`.`CourseID` = `c`.`CourseID`) AS `Avg_Feedback_Rating`, (select `f`.`FeedbackText` from `feedback` `f` where `f`.`CourseID` = `c`.`CourseID` order by `f`.`FeedbackDate` desc limit 1) AS `Recent_Feedback`, (select min(`a`.`DueDate`) from `assignments` `a` where `a`.`CourseID` = `c`.`CourseID` and `a`.`DueDate` >= current_timestamp()) AS `Next_Assignment_Due`, (select count(0) from `quizzes` `q` where `q`.`courseid` = `c`.`CourseID`) AS `Quiz_Count`, `c`.`Description` AS `Course_Description`, (select round(count(`s`.`SubmissionID`) * 100.0 / count(`a`.`AssignmentID`),1) from (`assignments` `a` left join `submissions` `s` on(`a`.`AssignmentID` = `s`.`AssignmentID`)) where `a`.`CourseID` = `c`.`CourseID`) AS `Submission_Rate_Pct` FROM `courses` AS `c` ;

-- --------------------------------------------------------

--
-- Structure for view `instructor_course_overview`
--
DROP TABLE IF EXISTS `instructor_course_overview`;

DROP VIEW IF EXISTS `instructor_course_overview`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `instructor_course_overview`  AS SELECT `c`.`CourseID` AS `CourseID`, `c`.`CourseName` AS `CourseName`, (select count(0) from `enrollments` `e` where `e`.`CourseID` = `c`.`CourseID`) AS `Enrolled_Students`, (select count(0) from `assignments` `a` where `a`.`CourseID` = `c`.`CourseID` and `a`.`DueDate` > current_timestamp()) AS `Active_Assignments`, (select count(0) from (`submissions` `s` join `assignments` `a` on(`s`.`AssignmentID` = `a`.`AssignmentID`)) where `a`.`CourseID` = `c`.`CourseID` and `s`.`Grade` is null) AS `Ungraded_Submissions`, (select round(avg(`f`.`Rating`),1) from `feedback` `f` where `f`.`CourseID` = `c`.`CourseID`) AS `Avg_Feedback_Rating`, (select substr(max(concat(`f`.`FeedbackDate`,'|',`f`.`FeedbackText`)),locate('|',max(concat(`f`.`FeedbackDate`,'|',`f`.`FeedbackText`))) + 1,50) from `feedback` `f` where `f`.`CourseID` = `c`.`CourseID`) AS `Recent_Feedback`, (select min(`a`.`DueDate`) from `assignments` `a` where `a`.`CourseID` = `c`.`CourseID` and `a`.`DueDate` > current_timestamp()) AS `Next_Assignment_Due`, (select count(0) from `quizzes` `q` where `q`.`courseid` = `c`.`CourseID`) AS `Quiz_Count`, `c`.`Description` AS `Course_Description`, round((select count(distinct `s`.`AssignmentID`) from (`submissions` `s` join `assignments` `a` on(`s`.`AssignmentID` = `a`.`AssignmentID`)) where `a`.`CourseID` = `c`.`CourseID`) / nullif((select count(0) from `assignments` `a` where `a`.`CourseID` = `c`.`CourseID`),0) * 100,1) AS `Submission_Rate_Pct` FROM `courses` AS `c` ;

-- --------------------------------------------------------

--
-- Structure for view `numberofcoursecreatedbyinstructor`
--
DROP TABLE IF EXISTS `numberofcoursecreatedbyinstructor`;

DROP VIEW IF EXISTS `numberofcoursecreatedbyinstructor`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `numberofcoursecreatedbyinstructor`  AS SELECT `u`.`UserID` AS `UserID`, `u`.`Username` AS `InstructorName`, count(`c`.`CourseID`) AS `NumberOfCourses` FROM (`users` `u` left join `courses` `c` on(`u`.`UserID` = `c`.`CreatedBy`)) WHERE `u`.`UserType` = 'Instructor' GROUP BY `u`.`UserID`, `u`.`Username` ;

-- --------------------------------------------------------

--
-- Structure for view `student_dashboard`
--
DROP TABLE IF EXISTS `student_dashboard`;

DROP VIEW IF EXISTS `student_dashboard`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `student_dashboard`  AS SELECT `e`.`UserID` AS `UserID`, `c`.`CourseID` AS `CourseID`, `c`.`CourseName` AS `CourseName`, `c`.`Description` AS `CourseDescription`, `u`.`Username` AS `InstructorName`, `u`.`Email` AS `InstructorEmail`, `e`.`EnrollmentDate` AS `EnrollmentDate`, `a`.`AssignmentTitle` AS `NextAssignment`, `a`.`DueDate` AS `NextAssignmentDueDate`, `s`.`Grade` AS `RecentGrade`, `s`.`SubmissionDate` AS `LastSubmissionDate`, `q`.`title` AS `NextQuiz`, `q`.`total_marks` AS `QuizTotalMarks`, `q`.`created_at` AS `QuizAvailableFrom`, `f`.`Rating` AS `FeedbackRating`, `f`.`FeedbackDate` AS `LastFeedbackDate` FROM ((((((`enrollments` `e` join `courses` `c` on(`e`.`CourseID` = `c`.`CourseID`)) join `users` `u` on(`c`.`CreatedBy` = `u`.`UserID`)) left join (select `a1`.`AssignmentID` AS `AssignmentID`,`a1`.`CourseID` AS `CourseID`,`a1`.`AssignmentTitle` AS `AssignmentTitle`,`a1`.`DueDate` AS `DueDate` from `assignments` `a1` where `a1`.`DueDate` >= curdate() order by `a1`.`DueDate`) `a` on(`a`.`CourseID` = `c`.`CourseID`)) left join (select `s1`.`SubmissionID` AS `SubmissionID`,`s1`.`AssignmentID` AS `AssignmentID`,`s1`.`UserID` AS `UserID`,`s1`.`SubmissionDate` AS `SubmissionDate`,`s1`.`Grade` AS `Grade` from `submissions` `s1` order by `s1`.`SubmissionDate` desc) `s` on(`s`.`UserID` = `e`.`UserID` and `s`.`AssignmentID` = `a`.`AssignmentID`)) left join (select `q1`.`quiz_id` AS `quiz_id`,`q1`.`courseid` AS `courseid`,`q1`.`title` AS `title`,`q1`.`description` AS `description`,`q1`.`total_marks` AS `total_marks`,`q1`.`created_at` AS `created_at`,`q1`.`updated_at` AS `updated_at` from `quizzes` `q1` where `q1`.`created_at` <= curdate() order by `q1`.`created_at`) `q` on(`q`.`courseid` = `c`.`CourseID`)) left join (select `f1`.`FeedbackID` AS `FeedbackID`,`f1`.`CourseID` AS `CourseID`,`f1`.`UserID` AS `UserID`,`f1`.`FeedbackText` AS `FeedbackText`,`f1`.`FeedbackDate` AS `FeedbackDate`,`f1`.`Rating` AS `Rating` from `feedback` `f1` order by `f1`.`FeedbackDate` desc) `f` on(`f`.`CourseID` = `c`.`CourseID` and `f`.`UserID` = `e`.`UserID`)) ;

-- --------------------------------------------------------

--
-- Structure for view `student_performance_summary`
--
DROP TABLE IF EXISTS `student_performance_summary`;

DROP VIEW IF EXISTS `student_performance_summary`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `student_performance_summary`  AS SELECT `u`.`UserID` AS `UserID`, `u`.`Username` AS `Student_Name`, (select count(0) from `enrollments` `e` where `e`.`UserID` = `u`.`UserID`) AS `Courses_Taken`, (select avg(`s`.`Grade`) from `submissions` `s` where `s`.`UserID` = `u`.`UserID`) AS `Avg_Grade`, (select count(0) from (`assignments` `a` left join `submissions` `s` on(`a`.`AssignmentID` = `s`.`AssignmentID` and `s`.`UserID` = `u`.`UserID`)) where `a`.`DueDate` < current_timestamp() and (`s`.`SubmissionID` is null or `s`.`SubmissionDate` > `a`.`DueDate`)) AS `Assignments_Missed`, (select round(count(distinct `f`.`CourseID`) * 100.0 / count(distinct `e`.`CourseID`),1) from (`enrollments` `e` left join `feedback` `f` on(`e`.`CourseID` = `f`.`CourseID` and `f`.`UserID` = `u`.`UserID`)) where `e`.`UserID` = `u`.`UserID`) AS `Feedback_Responsiveness`, (select count(0) from (`quiz_questions` `qq` join `quizzes` `q` on(`qq`.`quiz_id` = `q`.`quiz_id`)) where `q`.`courseid` in (select `enrollments`.`CourseID` from `enrollments` where `enrollments`.`UserID` = `u`.`UserID`)) AS `Total_Quiz_Questions`, (select `c`.`CourseName` from ((`courses` `c` join `enrollments` `e` on(`c`.`CourseID` = `e`.`CourseID`)) join `submissions` `s` on(`e`.`UserID` = `s`.`UserID`)) where `e`.`UserID` = `u`.`UserID` group by `c`.`CourseID` order by avg(`s`.`Grade`) desc limit 1) AS `Best_Performing_Course`, (select max(`submissions`.`SubmissionDate`) from `submissions` where `submissions`.`UserID` = `u`.`UserID`) AS `Last_Active_Date`, CASE WHEN (select count(0) from `enrollments` where `enrollments`.`UserID` = `u`.`UserID`) = 0 THEN 'Not Enrolled' WHEN (select count(0) from `submissions` where `submissions`.`UserID` = `u`.`UserID` AND `submissions`.`Grade` is null) > 3 THEN 'Missing Submissions' WHEN (select avg(`submissions`.`Grade`) from `submissions` where `submissions`.`UserID` = `u`.`UserID`) < 50 THEN 'Low Grades' ELSE 'On Track' END AS `Alert_Status` FROM `users` AS `u` WHERE `u`.`UserType` = 'Student' ;

-- --------------------------------------------------------

--
-- Structure for view `totalenrollmentspercourse`
--
DROP TABLE IF EXISTS `totalenrollmentspercourse`;

DROP VIEW IF EXISTS `totalenrollmentspercourse`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `totalenrollmentspercourse`  AS SELECT `c`.`CourseID` AS `CourseID`, `c`.`CourseName` AS `CourseName`, count(`e`.`EnrollmentID`) AS `TotalEnrollments` FROM (`courses` `c` left join `enrollments` `e` on(`c`.`CourseID` = `e`.`CourseID`)) GROUP BY `c`.`CourseID`, `c`.`CourseName` ;

-- --------------------------------------------------------

--
-- Structure for view `userassignmengrades`
--
DROP TABLE IF EXISTS `userassignmengrades`;

DROP VIEW IF EXISTS `userassignmengrades`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `userassignmengrades`  AS SELECT `u`.`UserID` AS `UserID`, `u`.`Username` AS `Username`, `c`.`CourseName` AS `CourseName`, `a`.`AssignmentTitle` AS `AssignmentTitle`, `s`.`Grade` AS `Grade`, CASE WHEN `s`.`Grade` >= 60 THEN 'Pass' ELSE 'Fail' END AS `STATUS` FROM (((`users` `u` join `submissions` `s` on(`u`.`UserID` = `s`.`UserID`)) join `assignments` `a` on(`s`.`AssignmentID` = `a`.`AssignmentID`)) join `courses` `c` on(`a`.`CourseID` = `c`.`CourseID`)) ;

-- --------------------------------------------------------

--
-- Structure for view `userassignmentdetails`
--
DROP TABLE IF EXISTS `userassignmentdetails`;

DROP VIEW IF EXISTS `userassignmentdetails`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `userassignmentdetails`  AS SELECT `u`.`UserID` AS `UserID`, `u`.`Username` AS `Username`, `c`.`CourseName` AS `CourseName`, `a`.`AssignmentTitle` AS `AssignmentTitle`, `getUserGradeDetails`(`u`.`UserID`,`c`.`CourseID`,`a`.`AssignmentID`) AS `Grade`, CASE WHEN `getUserGradeDetails`(`u`.`UserID`,`c`.`CourseID`,`a`.`AssignmentID`) is not null THEN 'Submitted' ELSE 'Pending' END AS `Status`, CASE WHEN `getUserGradeDetails`(`u`.`UserID`,`c`.`CourseID`,`a`.`AssignmentID`) >= 75 THEN 'Passed' WHEN `getUserGradeDetails`(`u`.`UserID`,`c`.`CourseID`,`a`.`AssignmentID`) is not null THEN 'Failed' ELSE 'N/A' END AS `Mark` FROM ((((`users` `u` join `enrollments` `e` on(`u`.`UserID` = `e`.`UserID`)) join `courses` `c` on(`e`.`CourseID` = `c`.`CourseID`)) join `assignments` `a` on(`c`.`CourseID` = `a`.`CourseID`)) left join `submissions` `s` on(`u`.`UserID` = `s`.`UserID` and `a`.`AssignmentID` = `s`.`AssignmentID`)) ;

-- --------------------------------------------------------

--
-- Structure for view `userenrollmentswithinstructor`
--
DROP TABLE IF EXISTS `userenrollmentswithinstructor`;

DROP VIEW IF EXISTS `userenrollmentswithinstructor`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `userenrollmentswithinstructor`  AS SELECT `e`.`EnrollmentID` AS `EnrollmentID`, `u`.`UserID` AS `UserID`, `u`.`Username` AS `Username`, `c`.`CourseID` AS `CourseID`, `c`.`CourseName` AS `CourseName`, `i`.`Username` AS `InstructorName`, `e`.`EnrollmentDate` AS `EnrollmentDate` FROM (((`enrollments` `e` join `users` `u` on(`e`.`UserID` = `u`.`UserID`)) join `courses` `c` on(`e`.`CourseID` = `c`.`CourseID`)) join `users` `i` on(`c`.`CreatedBy` = `i`.`UserID`)) ;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `assignments`
--
ALTER TABLE `assignments`
  ADD PRIMARY KEY (`AssignmentID`),
  ADD KEY `CourseID` (`CourseID`);

--
-- Indexes for table `audit_log`
--
ALTER TABLE `audit_log`
  ADD PRIMARY KEY (`log_id`);

--
-- Indexes for table `courses`
--
ALTER TABLE `courses`
  ADD PRIMARY KEY (`CourseID`),
  ADD KEY `CreatedBy` (`CreatedBy`);

--
-- Indexes for table `enrollments`
--
ALTER TABLE `enrollments`
  ADD PRIMARY KEY (`EnrollmentID`),
  ADD KEY `UserID` (`UserID`),
  ADD KEY `CourseID` (`CourseID`);

--
-- Indexes for table `feedback`
--
ALTER TABLE `feedback`
  ADD PRIMARY KEY (`FeedbackID`),
  ADD KEY `CourseID` (`CourseID`),
  ADD KEY `UserID` (`UserID`);

--
-- Indexes for table `lessons`
--
ALTER TABLE `lessons`
  ADD PRIMARY KEY (`LessonID`),
  ADD KEY `CourseID` (`CourseID`);

--
-- Indexes for table `quizzes`
--
ALTER TABLE `quizzes`
  ADD PRIMARY KEY (`quiz_id`),
  ADD KEY `courseid` (`courseid`);

--
-- Indexes for table `quiz_answers`
--
ALTER TABLE `quiz_answers`
  ADD PRIMARY KEY (`answer_id`),
  ADD KEY `question_id` (`question_id`) USING BTREE;

--
-- Indexes for table `quiz_questions`
--
ALTER TABLE `quiz_questions`
  ADD PRIMARY KEY (`question_id`),
  ADD KEY `quiz_id` (`quiz_id`);

--
-- Indexes for table `submissions`
--
ALTER TABLE `submissions`
  ADD PRIMARY KEY (`SubmissionID`),
  ADD KEY `AssignmentID` (`AssignmentID`),
  ADD KEY `UserID` (`UserID`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`UserID`),
  ADD UNIQUE KEY `Username` (`Username`),
  ADD UNIQUE KEY `Email` (`Email`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `assignments`
--
ALTER TABLE `assignments`
  MODIFY `AssignmentID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `audit_log`
--
ALTER TABLE `audit_log`
  MODIFY `log_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=18;

--
-- AUTO_INCREMENT for table `courses`
--
ALTER TABLE `courses`
  MODIFY `CourseID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT for table `enrollments`
--
ALTER TABLE `enrollments`
  MODIFY `EnrollmentID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT for table `feedback`
--
ALTER TABLE `feedback`
  MODIFY `FeedbackID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `lessons`
--
ALTER TABLE `lessons`
  MODIFY `LessonID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT for table `quizzes`
--
ALTER TABLE `quizzes`
  MODIFY `quiz_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- AUTO_INCREMENT for table `quiz_answers`
--
ALTER TABLE `quiz_answers`
  MODIFY `answer_id` int(20) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=81;

--
-- AUTO_INCREMENT for table `quiz_questions`
--
ALTER TABLE `quiz_questions`
  MODIFY `question_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- AUTO_INCREMENT for table `submissions`
--
ALTER TABLE `submissions`
  MODIFY `SubmissionID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `UserID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=22;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `assignments`
--
ALTER TABLE `assignments`
  ADD CONSTRAINT `assignments_ibfk_1` FOREIGN KEY (`CourseID`) REFERENCES `courses` (`CourseID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `courses`
--
ALTER TABLE `courses`
  ADD CONSTRAINT `courses_ibfk_1` FOREIGN KEY (`CreatedBy`) REFERENCES `users` (`UserID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `enrollments`
--
ALTER TABLE `enrollments`
  ADD CONSTRAINT `enrollments_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `users` (`UserID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `enrollments_ibfk_2` FOREIGN KEY (`CourseID`) REFERENCES `courses` (`CourseID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `feedback`
--
ALTER TABLE `feedback`
  ADD CONSTRAINT `feedback_ibfk_1` FOREIGN KEY (`CourseID`) REFERENCES `courses` (`CourseID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `feedback_ibfk_2` FOREIGN KEY (`UserID`) REFERENCES `users` (`UserID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `lessons`
--
ALTER TABLE `lessons`
  ADD CONSTRAINT `lessons_ibfk_1` FOREIGN KEY (`CourseID`) REFERENCES `courses` (`CourseID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `quizzes`
--
ALTER TABLE `quizzes`
  ADD CONSTRAINT `quizzes_ibfk_1` FOREIGN KEY (`courseid`) REFERENCES `courses` (`CourseID`) ON DELETE CASCADE;

--
-- Constraints for table `quiz_answers`
--
ALTER TABLE `quiz_answers`
  ADD CONSTRAINT `fk_question_id` FOREIGN KEY (`question_id`) REFERENCES `quiz_questions` (`question_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `quiz_questions`
--
ALTER TABLE `quiz_questions`
  ADD CONSTRAINT `quiz_questions_ibfk_1` FOREIGN KEY (`quiz_id`) REFERENCES `quizzes` (`quiz_id`);

--
-- Constraints for table `submissions`
--
ALTER TABLE `submissions`
  ADD CONSTRAINT `submissions_ibfk_1` FOREIGN KEY (`AssignmentID`) REFERENCES `assignments` (`AssignmentID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `submissions_ibfk_2` FOREIGN KEY (`UserID`) REFERENCES `users` (`UserID`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
