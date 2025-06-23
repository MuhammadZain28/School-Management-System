-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
-- -----------------------------------------------------
-- Schema lms1
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema lms1
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `lms1` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci ;
USE `lms1` ;

-- -----------------------------------------------------
-- Table `lms1`.`announcements`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `lms1`.`announcements` ;

CREATE TABLE IF NOT EXISTS `lms1`.`announcements` (
  `announcement_id` INT NOT NULL AUTO_INCREMENT,
  `title` VARCHAR(100) NOT NULL,
  `message` TEXT NOT NULL,
  `announcement_for` VARCHAR(20) NULL DEFAULT NULL,
  `announcement_date` DATE NULL DEFAULT NULL,
  PRIMARY KEY (`announcement_id`))
ENGINE = InnoDB
AUTO_INCREMENT = 2
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `lms1`.`teachers`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `lms1`.`teachers` ;

CREATE TABLE IF NOT EXISTS `lms1`.`teachers` (
  `teacher_id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(100) NOT NULL,
  `contact` VARCHAR(11) NOT NULL,
  `joining_date` DATE NOT NULL,
  `salary` INT NULL DEFAULT NULL,
  PRIMARY KEY (`teacher_id`),
  UNIQUE INDEX `contact` (`contact` ASC) VISIBLE)
ENGINE = InnoDB
AUTO_INCREMENT = 7
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `lms1`.`courses`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `lms1`.`courses` ;

CREATE TABLE IF NOT EXISTS `lms1`.`courses` (
  `course_id` INT NOT NULL AUTO_INCREMENT,
  `course_name` VARCHAR(100) NOT NULL,
  `type` VARCHAR(10) NULL DEFAULT NULL,
  `teacher_id` INT NULL DEFAULT NULL,
  `added_on` DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`course_id`),
  INDEX `teacher_id` (`teacher_id` ASC) VISIBLE,
  CONSTRAINT `courses_ibfk_1`
    FOREIGN KEY (`teacher_id`)
    REFERENCES `lms1`.`teachers` (`teacher_id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 8
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `lms1`.`batches`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `lms1`.`batches` ;

CREATE TABLE IF NOT EXISTS `lms1`.`batches` (
  `batch_id` INT NOT NULL AUTO_INCREMENT,
  `batch_name` VARCHAR(50) NOT NULL,
  `start_date` DATE NOT NULL,
  `end_date` DATE NULL DEFAULT NULL,
  `course_id` INT NULL DEFAULT NULL,
  `fee` DECIMAL(10,2) NULL DEFAULT NULL,
  PRIMARY KEY (`batch_id`),
  INDEX `course_id` (`course_id` ASC) VISIBLE,
  CONSTRAINT `batches_ibfk_1`
    FOREIGN KEY (`course_id`)
    REFERENCES `lms1`.`courses` (`course_id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 4
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `lms1`.`classes`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `lms1`.`classes` ;

CREATE TABLE IF NOT EXISTS `lms1`.`classes` (
  `class_id` INT NOT NULL AUTO_INCREMENT,
  `class_name` VARCHAR(10) NULL DEFAULT NULL,
  PRIMARY KEY (`class_id`))
ENGINE = InnoDB
AUTO_INCREMENT = 17
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `lms1`.`students`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `lms1`.`students` ;

CREATE TABLE IF NOT EXISTS `lms1`.`students` (
  `student_id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(100) NOT NULL,
  `roll_no` VARCHAR(10) NOT NULL,
  `age` INT NULL DEFAULT 15,
  `gender` VARCHAR(10) NULL DEFAULT NULL,
  `address` TEXT NULL DEFAULT NULL,
  `admission_date` DATE NOT NULL,
  `batch_id` INT NULL DEFAULT NULL,
  `class_id` INT NULL DEFAULT NULL,
  `contact` VARCHAR(15) NULL DEFAULT '03XX-XXXXXXXXX',
  PRIMARY KEY (`student_id`),
  UNIQUE INDEX `roll_no` (`roll_no` ASC) VISIBLE,
  INDEX `batch_id` (`batch_id` ASC) VISIBLE,
  INDEX `class_id` (`class_id` ASC) VISIBLE,
  CONSTRAINT `students_ibfk_1`
    FOREIGN KEY (`batch_id`)
    REFERENCES `lms1`.`batches` (`batch_id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE,
  CONSTRAINT `students_ibfk_2`
    FOREIGN KEY (`class_id`)
    REFERENCES `lms1`.`classes` (`class_id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 164
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `lms1`.`attendance`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `lms1`.`attendance` ;

CREATE TABLE IF NOT EXISTS `lms1`.`attendance` (
  `attendance_id` INT NOT NULL AUTO_INCREMENT,
  `student_id` INT NULL DEFAULT NULL,
  `class_id` INT NULL DEFAULT NULL,
  `batch_id` INT NULL DEFAULT NULL,
  `status` CHAR(1) NULL DEFAULT 'P',
  `date` DATE NOT NULL,
  PRIMARY KEY (`attendance_id`),
  INDEX `student_id` (`student_id` ASC) VISIBLE,
  INDEX `class_id` (`class_id` ASC) VISIBLE,
  INDEX `batch_id` (`batch_id` ASC) VISIBLE,
  CONSTRAINT `attendance_ibfk_1`
    FOREIGN KEY (`student_id`)
    REFERENCES `lms1`.`students` (`student_id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `attendance_ibfk_2`
    FOREIGN KEY (`class_id`)
    REFERENCES `lms1`.`classes` (`class_id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `attendance_ibfk_3`
    FOREIGN KEY (`batch_id`)
    REFERENCES `lms1`.`batches` (`batch_id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 33
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `lms1`.`class_schedule`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `lms1`.`class_schedule` ;

CREATE TABLE IF NOT EXISTS `lms1`.`class_schedule` (
  `schedule_id` INT NOT NULL AUTO_INCREMENT,
  `day_of_week` ENUM('Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday') NOT NULL,
  `start_time` TIME NOT NULL,
  `end_time` TIME NOT NULL,
  `teacher_id` INT NULL DEFAULT NULL,
  `class_id` INT NULL DEFAULT NULL,
  `course_id` INT NULL DEFAULT NULL,
  PRIMARY KEY (`schedule_id`),
  INDEX `teacher_id` (`teacher_id` ASC) VISIBLE,
  INDEX `class_id` (`class_id` ASC) VISIBLE,
  INDEX `course_id` (`course_id` ASC) VISIBLE,
  CONSTRAINT `class_schedule_ibfk_1`
    FOREIGN KEY (`teacher_id`)
    REFERENCES `lms1`.`teachers` (`teacher_id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `class_schedule_ibfk_2`
    FOREIGN KEY (`class_id`)
    REFERENCES `lms1`.`classes` (`class_id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `class_schedule_ibfk_3`
    FOREIGN KEY (`course_id`)
    REFERENCES `lms1`.`courses` (`course_id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 11
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `lms1`.`course_materials`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `lms1`.`course_materials` ;

CREATE TABLE IF NOT EXISTS `lms1`.`course_materials` (
  `material_id` INT NOT NULL AUTO_INCREMENT,
  `course_id` INT NULL DEFAULT NULL,
  `title` VARCHAR(100) NOT NULL,
  `file_link` TEXT NULL DEFAULT NULL,
  `uploaded_date` DATE NULL DEFAULT NULL,
  PRIMARY KEY (`material_id`),
  INDEX `course_id` (`course_id` ASC) VISIBLE,
  CONSTRAINT `course_materials_ibfk_1`
    FOREIGN KEY (`course_id`)
    REFERENCES `lms1`.`courses` (`course_id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `lms1`.`events`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `lms1`.`events` ;

CREATE TABLE IF NOT EXISTS `lms1`.`events` (
  `event_id` INT NOT NULL AUTO_INCREMENT,
  `event_name` VARCHAR(100) NOT NULL,
  `date` DATE NOT NULL,
  `description` TEXT NULL DEFAULT NULL,
  PRIMARY KEY (`event_id`))
ENGINE = InnoDB
AUTO_INCREMENT = 5
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `lms1`.`exam_schedule`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `lms1`.`exam_schedule` ;

CREATE TABLE IF NOT EXISTS `lms1`.`exam_schedule` (
  `exam_id` INT NOT NULL AUTO_INCREMENT,
  `course_id` INT NULL DEFAULT NULL,
  `class_id` INT NULL DEFAULT NULL,
  `date` DATE NOT NULL,
  `total_marks` INT NULL DEFAULT NULL,
  `duration` VARCHAR(20) NULL DEFAULT NULL,
  PRIMARY KEY (`exam_id`),
  INDEX `course_id` (`course_id` ASC) VISIBLE,
  INDEX `class_id` (`class_id` ASC) VISIBLE,
  CONSTRAINT `exam_schedule_ibfk_1`
    FOREIGN KEY (`course_id`)
    REFERENCES `lms1`.`courses` (`course_id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `exam_schedule_ibfk_2`
    FOREIGN KEY (`class_id`)
    REFERENCES `lms1`.`classes` (`class_id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 3
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `lms1`.`fee_records`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `lms1`.`fee_records` ;

CREATE TABLE IF NOT EXISTS `lms1`.`fee_records` (
  `fee_id` INT NOT NULL AUTO_INCREMENT,
  `student_id` INT NULL DEFAULT NULL,
  `amount_paid` DECIMAL(10,2) NULL DEFAULT NULL,
  `due_amount` DECIMAL(10,2) NULL DEFAULT NULL,
  `payment_date` DATE NULL DEFAULT NULL,
  `status` TINYINT(1) NULL DEFAULT NULL,
  PRIMARY KEY (`fee_id`),
  INDEX `student_id` (`student_id` ASC) VISIBLE,
  CONSTRAINT `fee_records_ibfk_1`
    FOREIGN KEY (`student_id`)
    REFERENCES `lms1`.`students` (`student_id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 118
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `lms1`.`logins`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `lms1`.`logins` ;

CREATE TABLE IF NOT EXISTS `lms1`.`logins` (
  `user_id` INT NOT NULL AUTO_INCREMENT,
  `username` VARCHAR(50) NOT NULL,
  `password` VARCHAR(255) NOT NULL,
  `role` VARCHAR(20) NULL DEFAULT NULL,
  PRIMARY KEY (`user_id`),
  UNIQUE INDEX `username` (`username` ASC) VISIBLE)
ENGINE = InnoDB
AUTO_INCREMENT = 11
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `lms1`.`month_log`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `lms1`.`month_log` ;

CREATE TABLE IF NOT EXISTS `lms1`.`month_log` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `log_date` DATE NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 3
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `lms1`.`notifications`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `lms1`.`notifications` ;

CREATE TABLE IF NOT EXISTS `lms1`.`notifications` (
  `notif_id` INT NOT NULL AUTO_INCREMENT,
  `user_id` INT NULL DEFAULT NULL,
  `message` TEXT NOT NULL,
  `seen_status` TINYINT(1) NULL DEFAULT '0',
  `date` DATE NULL DEFAULT NULL,
  PRIMARY KEY (`notif_id`),
  INDEX `user_id` (`user_id` ASC) VISIBLE,
  CONSTRAINT `notifications_ibfk_1`
    FOREIGN KEY (`user_id`)
    REFERENCES `lms1`.`logins` (`user_id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `lms1`.`payments`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `lms1`.`payments` ;

CREATE TABLE IF NOT EXISTS `lms1`.`payments` (
  `PaymentID` INT NOT NULL AUTO_INCREMENT,
  `PaymentType` VARCHAR(50) NULL DEFAULT NULL,
  `Name` VARCHAR(100) NULL DEFAULT NULL,
  `ToEntity` VARCHAR(100) NULL DEFAULT NULL,
  `Contact` VARCHAR(50) NULL DEFAULT NULL,
  `PaymentDate` DATE NULL DEFAULT NULL,
  `Amount_paid` INT NULL DEFAULT NULL,
  PRIMARY KEY (`PaymentID`))
ENGINE = InnoDB
AUTO_INCREMENT = 3
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `lms1`.`results`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `lms1`.`results` ;

CREATE TABLE IF NOT EXISTS `lms1`.`results` (
  `result_id` INT NOT NULL AUTO_INCREMENT,
  `student_id` INT NULL DEFAULT NULL,
  `exam_id` INT NULL DEFAULT NULL,
  `total_marks` INT NULL DEFAULT NULL,
  `obtained_marks` INT NULL DEFAULT NULL,
  `grade` CHAR(2) NULL DEFAULT NULL,
  `remarks` TEXT NULL DEFAULT NULL,
  PRIMARY KEY (`result_id`),
  INDEX `student_id` (`student_id` ASC) VISIBLE,
  INDEX `exam_id` (`exam_id` ASC) VISIBLE,
  CONSTRAINT `results_ibfk_1`
    FOREIGN KEY (`student_id`)
    REFERENCES `lms1`.`students` (`student_id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `results_ibfk_2`
    FOREIGN KEY (`exam_id`)
    REFERENCES `lms1`.`exam_schedule` (`exam_id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 2
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `lms1`.`salaries`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `lms1`.`salaries` ;

CREATE TABLE IF NOT EXISTS `lms1`.`salaries` (
  `salary_id` INT NOT NULL AUTO_INCREMENT,
  `teacher_id` INT NULL DEFAULT NULL,
  `month` VARCHAR(20) NULL DEFAULT NULL,
  `amount_paid` DECIMAL(10,2) NULL DEFAULT NULL,
  `payment_date` DATE NULL DEFAULT NULL,
  PRIMARY KEY (`salary_id`),
  INDEX `teacher_id` (`teacher_id` ASC) VISIBLE,
  CONSTRAINT `salaries_ibfk_1`
    FOREIGN KEY (`teacher_id`)
    REFERENCES `lms1`.`teachers` (`teacher_id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `lms1`.`teacher_qualifications`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `lms1`.`teacher_qualifications` ;

CREATE TABLE IF NOT EXISTS `lms1`.`teacher_qualifications` (
  `qualification_id` INT NOT NULL AUTO_INCREMENT,
  `teacher_id` INT NULL DEFAULT NULL,
  `degree_name` VARCHAR(100) NOT NULL,
  `institute` VARCHAR(100) NULL DEFAULT NULL,
  `year` INT NULL DEFAULT NULL,
  PRIMARY KEY (`qualification_id`),
  INDEX `teacher_id` (`teacher_id` ASC) VISIBLE,
  CONSTRAINT `teacher_qualifications_ibfk_1`
    FOREIGN KEY (`teacher_id`)
    REFERENCES `lms1`.`teachers` (`teacher_id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 7
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `lms1`.`user_logs`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `lms1`.`user_logs` ;

CREATE TABLE IF NOT EXISTS `lms1`.`user_logs` (
  `log_id` INT NOT NULL AUTO_INCREMENT,
  `user_id` INT NULL DEFAULT NULL,
  `action` TEXT NOT NULL,
  `timestamp` DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`log_id`),
  INDEX `user_id` (`user_id` ASC) VISIBLE,
  CONSTRAINT `user_logs_ibfk_1`
    FOREIGN KEY (`user_id`)
    REFERENCES `lms1`.`logins` (`user_id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;

USE `lms1` ;

-- -----------------------------------------------------
-- Placeholder table for view `lms1`.`academic_year_student_performance`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `lms1`.`academic_year_student_performance` (`Roll No` INT, `Student` INT, `Academic Year` INT, `Percentage` INT);

-- -----------------------------------------------------
-- Placeholder table for view `lms1`.`finance_report`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `lms1`.`finance_report` (`Year` INT, `Total Collected` INT, `Total Due` INT);

-- -----------------------------------------------------
-- Placeholder table for view `lms1`.`monthly_attendance_report`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `lms1`.`monthly_attendance_report` (`Roll No` INT, `Students` INT, `Month` INT, `Year` INT, `Present` INT, `Absent` INT, `Leaves` INT, `Total Classes` INT);

-- -----------------------------------------------------
-- Placeholder table for view `lms1`.`student_result_report`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `lms1`.`student_result_report` (`student_id` INT, `name` INT, `month` INT, `year` INT, `total_obtained` INT, `total_marks` INT, `percentage` INT);

-- -----------------------------------------------------
-- Placeholder table for view `lms1`.`teacher_performance_report`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `lms1`.`teacher_performance_report` (`Teacher` INT, `Course` INT, `Total Students` INT, `Pass Percentage` INT);

-- -----------------------------------------------------
-- procedure finance_report_by_year
-- -----------------------------------------------------

USE `lms1`;
DROP procedure IF EXISTS `lms1`.`finance_report_by_year`;

DELIMITER $$
USE `lms1`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `finance_report_by_year`(in selected_year int)
begin
    select 
        year(f.payment_date) as Year,
        sum(f.amount_paid) as "Total Collected",
        sum(f.due_amount) as "Total Due"
    from fee_records f
    where year(f.payment_date) = selected_year
    group by year(f.payment_date);
end$$

DELIMITER ;

-- -----------------------------------------------------
-- View `lms1`.`academic_year_student_performance`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `lms1`.`academic_year_student_performance`;
DROP VIEW IF EXISTS `lms1`.`academic_year_student_performance` ;
USE `lms1`;
CREATE  OR REPLACE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `lms1`.`academic_year_student_performance` AS select `s`.`roll_no` AS `Roll No`,`s`.`name` AS `Student`,year(`e`.`date`) AS `Academic Year`,avg(((`r`.`obtained_marks` / `e`.`total_marks`) * 100)) AS `Percentage` from ((`lms1`.`results` `r` join `lms1`.`students` `s` on((`r`.`student_id` = `s`.`student_id`))) join `lms1`.`exam_schedule` `e` on((`r`.`exam_id` = `e`.`exam_id`))) group by `s`.`student_id`,year(`e`.`date`);

-- -----------------------------------------------------
-- View `lms1`.`finance_report`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `lms1`.`finance_report`;
DROP VIEW IF EXISTS `lms1`.`finance_report` ;
USE `lms1`;
CREATE  OR REPLACE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `lms1`.`finance_report` AS select year(`f`.`payment_date`) AS `Year`,sum(`f`.`amount_paid`) AS `Total Collected`,sum(`f`.`due_amount`) AS `Total Due` from `lms1`.`fee_records` `f` group by year(`f`.`payment_date`);

-- -----------------------------------------------------
-- View `lms1`.`monthly_attendance_report`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `lms1`.`monthly_attendance_report`;
DROP VIEW IF EXISTS `lms1`.`monthly_attendance_report` ;
USE `lms1`;
CREATE  OR REPLACE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `lms1`.`monthly_attendance_report` AS select `s`.`roll_no` AS `Roll No`,`s`.`name` AS `Students`,month(`a`.`date`) AS `Month`,year(`a`.`date`) AS `Year`,sum((case when (`a`.`status` = 'P') then 1 else 0 end)) AS `Present`,sum((case when (`a`.`status` = 'A') then 1 else 0 end)) AS `Absent`,sum((case when (`a`.`status` = 'L') then 1 else 0 end)) AS `Leaves`,count(0) AS `Total Classes` from ((`lms1`.`attendance` `a` join `lms1`.`students` `s` on((`a`.`student_id` = `s`.`student_id`))) join `lms1`.`classes` `c` on((`a`.`class_id` = `c`.`class_id`))) group by `s`.`student_id`,`s`.`name`,month(`a`.`date`),year(`a`.`date`);

-- -----------------------------------------------------
-- View `lms1`.`student_result_report`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `lms1`.`student_result_report`;
DROP VIEW IF EXISTS `lms1`.`student_result_report` ;
USE `lms1`;
CREATE  OR REPLACE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `lms1`.`student_result_report` AS select `r`.`student_id` AS `student_id`,`s`.`name` AS `name`,month(`e`.`date`) AS `month`,year(`e`.`date`) AS `year`,sum(`r`.`obtained_marks`) AS `total_obtained`,sum(`e`.`total_marks`) AS `total_marks`,round(((sum(`r`.`obtained_marks`) / sum(`e`.`total_marks`)) * 100),2) AS `percentage` from ((`lms1`.`results` `r` join `lms1`.`students` `s` on((`r`.`student_id` = `s`.`student_id`))) join `lms1`.`exam_schedule` `e` on((`r`.`exam_id` = `e`.`exam_id`))) group by `s`.`student_id`,`s`.`name`,month(`e`.`date`),year(`e`.`date`);

-- -----------------------------------------------------
-- View `lms1`.`teacher_performance_report`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `lms1`.`teacher_performance_report`;
DROP VIEW IF EXISTS `lms1`.`teacher_performance_report` ;
USE `lms1`;
CREATE  OR REPLACE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `lms1`.`teacher_performance_report` AS select `t`.`name` AS `Teacher`,`c`.`course_name` AS `Course`,count(0) AS `Total Students`,round(((sum((case when (((`r`.`obtained_marks` / `e`.`total_marks`) * 100) > 50) then 1 else 0 end)) / count(0)) * 100),2) AS `Pass Percentage` from (((`lms1`.`results` `r` join `lms1`.`exam_schedule` `e` on((`e`.`exam_id` = `r`.`exam_id`))) join `lms1`.`courses` `c` on((`e`.`course_id` = `c`.`course_id`))) join `lms1`.`teachers` `t` on((`c`.`teacher_id` = `t`.`teacher_id`))) group by `t`.`teacher_id`,`t`.`name`,`c`.`course_id`,`c`.`course_name`;
USE `lms1`;

DELIMITER $$

USE `lms1`$$
CREATE
DEFINER=`root`@`localhost`
TRIGGER `lms1`.`generate_fees`
AFTER INSERT ON `lms1`.`month_log`
FOR EACH ROW
BEGIN
    INSERT INTO fee_records (student_id, amount_paid, due_amount, payment_date, status)
    SELECT s.student_id, 0.00, b.fee, NEW.log_date, FALSE
    FROM students s JOIN batches b ON s.batch_id = b.batch_id;
END$$


DELIMITER ;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
