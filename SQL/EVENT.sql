delimiter $$
$$
CREATE EVENT `UpdateReservations`
ON SCHEDULE EVERY 1 MINUTE
DO
UPDATE `reservation` SET `Status` = 'Expired' WHERE `reservation`.`Status` = 'Waiting' AND CURRENT_TIMESTAMP > ADDDATE(`reservation`.`Beg_Datetime`,  INTERVAL getLeeway() MINUTE);
UPDATE `reservation` SET `Status` = 'Occupied' WHERE `reservation`.`Status` = 'Active' AND CURRENT_TIMESTAMP > ADDDATE(ADDDATE(`reservation`.`Beg_Datetime`, INTERVAL `reservation`.`Duration` HOUR),  INTERVAL  getLeeway() MINUTE);
$$