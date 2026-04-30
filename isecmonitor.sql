-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Хост: 127.0.0.1
-- Время создания: Апр 30 2026 г., 17:35
-- Версия сервера: 10.4.32-MariaDB
-- Версия PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- База данных: `isecmonitor`
--

DELIMITER $$
--
-- Процедуры
--
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_add_event` (IN `p_resource_id` INT, IN `p_user_id` INT, IN `p_event_type` VARCHAR(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci, IN `p_event_description` TEXT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci, IN `p_severity` VARCHAR(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci)   BEGIN
    INSERT INTO events (
        resource_id, user_id, event_type,
        event_description, severity, event_time
    )
    VALUES (
        p_resource_id, p_user_id, p_event_type,
        p_event_description, p_severity, NOW()
    );

    INSERT INTO logs (user_id, action, log_time)
    VALUES (
        p_user_id,
        CONCAT('Добавлено событие мониторинга: ', p_event_type),
        NOW()
    );
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_create_incident` (IN `p_event_id` INT, IN `p_threat_id` INT, IN `p_status` VARCHAR(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci)   BEGIN
    IF NOT EXISTS (SELECT 1 FROM events WHERE event_id = p_event_id) THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Событие не найдено';
    END IF;

    IF NOT EXISTS (SELECT 1 FROM threats WHERE threat_id = p_threat_id) THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Угроза не найдена';
    END IF;

    INSERT INTO incidents (
        event_id, threat_id, status, detected_at, updated_at
    )
    VALUES (
        p_event_id, p_threat_id, p_status, NOW(), NOW()
    );
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_register_user` (IN `p_login` VARCHAR(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci, IN `p_salt` VARCHAR(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci, IN `p_hash` VARCHAR(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci, IN `p_fullname` VARCHAR(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci, IN `p_email` VARCHAR(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci, IN `p_role` VARCHAR(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci)   BEGIN
    IF EXISTS (
        SELECT 1
        FROM users
        WHERE login = p_login COLLATE utf8mb4_unicode_ci
    ) THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Логин уже существует';
    END IF;

    IF EXISTS (
        SELECT 1
        FROM users
        WHERE email = p_email COLLATE utf8mb4_unicode_ci
    ) THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Email уже существует';
    END IF;

    INSERT INTO users (
        login, password_hash, password_salt,
        full_name, email, role_name, is_active, created_at
    )
    VALUES (
        p_login, p_hash, p_salt,
        p_fullname, p_email, p_role, 1, NOW()
    );
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_update_incident_status` (IN `p_incident_id` INT, IN `p_new_status` VARCHAR(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci)   BEGIN
    UPDATE incidents
    SET status = p_new_status,
        resolved_at = CASE
            WHEN p_new_status = 'resolved' THEN NOW()
            ELSE resolved_at
        END
    WHERE incident_id = p_incident_id;
END$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Структура таблицы `events`
--

CREATE TABLE `events` (
  `event_id` int(11) NOT NULL,
  `resource_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `event_type` varchar(100) NOT NULL,
  `event_description` text DEFAULT NULL,
  `severity` varchar(20) NOT NULL,
  `event_time` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Дамп данных таблицы `events`
--

INSERT INTO `events` (`event_id`, `resource_id`, `user_id`, `event_type`, `event_description`, `severity`, `event_time`) VALUES
(1, 1, 3, 'Неудачная попытка входа', 'Зафиксировано 5 неудачных попыток входа подряд', 'high', '2026-04-20 10:47:47'),
(2, 2, 2, 'Изменение привилегий', 'Обнаружено изменение прав доступа к БД', 'critical', '2026-04-21 10:47:47'),
(3, 3, 1, 'Сканирование портов', 'Подозрительная сетевая активность на границе сети', 'medium', '2026-04-22 10:47:47'),
(4, 1, 2, 'Подозрительное изменение конфигурации', 'На сервере обнаружено изменение системных параметров безопасности', 'high', '2026-04-22 10:47:47'),
(5, 1, 2, 'Тестовое событие', 'Описание события', 'low', '2026-04-22 11:23:23'),
(6, 1, 2, 'Подозрительное изменение конфигурации', 'На сервере обнаружено изменение системных параметров безопасности', 'high', '2026-04-22 11:26:56'),
(7, 1, 2, 'Подозрительное изменение конфигурации', 'На сервере обнаружено изменение системных параметров безопасности', 'high', '2026-04-22 11:27:19'),
(8, 1, 2, 'Тестовое событие', 'Проверка добавления', 'low', '2026-04-22 23:49:53'),
(9, 2, 1, 'Test', 'Test', 'low', '2026-04-23 00:59:53'),
(10, 2, 1, 'test', 'test', 'low', '2026-04-23 22:50:02'),
(11, 2, 1, 'Test', 'Test', 'high', '2026-04-24 00:11:57');

-- --------------------------------------------------------

--
-- Структура таблицы `incidents`
--

CREATE TABLE `incidents` (
  `incident_id` int(11) NOT NULL,
  `event_id` int(11) NOT NULL,
  `threat_id` int(11) NOT NULL,
  `status` varchar(50) NOT NULL,
  `detected_at` datetime NOT NULL DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT NULL,
  `resolved_at` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Дамп данных таблицы `incidents`
--

INSERT INTO `incidents` (`incident_id`, `event_id`, `threat_id`, `status`, `detected_at`, `updated_at`, `resolved_at`) VALUES
(1, 1, 1, 'new', '2026-04-20 10:47:47', '2026-04-23 22:50:16', '2026-04-22 23:51:37'),
(2, 2, 2, 'in_progress', '2026-04-21 10:47:47', '2026-04-24 00:12:09', NULL),
(3, 3, 3, 'resolved', '2026-04-22 10:47:47', '2026-04-22 10:47:47', '2026-04-22 10:47:47'),
(4, 4, 2, 'new', '2026-04-22 10:47:47', '2026-04-22 10:47:47', NULL),
(5, 1, 1, 'new', '2026-04-22 11:35:37', '2026-04-22 11:35:37', NULL);

--
-- Триггеры `incidents`
--
DELIMITER $$
CREATE TRIGGER `trg_incidents_audit` AFTER UPDATE ON `incidents` FOR EACH ROW BEGIN
    IF OLD.status <> NEW.status THEN
        INSERT INTO logs (user_id, action, log_time)
        VALUES (
            NULL,
            CONCAT(
                'Изменён статус инцидента #', NEW.incident_id,
                ': ', OLD.status, ' -> ', NEW.status
            ),
            NOW()
        );
    END IF;
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `trg_incidents_updated_at` BEFORE UPDATE ON `incidents` FOR EACH ROW BEGIN
    SET NEW.updated_at = NOW();
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Структура таблицы `logs`
--

CREATE TABLE `logs` (
  `log_id` int(11) NOT NULL,
  `user_id` int(11) DEFAULT NULL,
  `action` varchar(255) NOT NULL,
  `log_time` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Дамп данных таблицы `logs`
--

INSERT INTO `logs` (`log_id`, `user_id`, `action`, `log_time`) VALUES
(1, 1, 'Создана учётная запись аналитика', '2026-04-19 10:47:47'),
(2, 2, 'Выполнен анализ события безопасности', '2026-04-21 10:47:47'),
(3, 3, 'Добавлено новое событие мониторинга', '2026-04-22 10:47:47'),
(4, 2, 'Добавлено событие мониторинга: Подозрительное изменение конфигурации', '2026-04-22 10:47:47'),
(5, NULL, 'Изменён статус инцидента #1: new -> in_progress', '2026-04-22 10:47:47'),
(6, NULL, 'Изменён статус инцидента #1: in_progress -> resolved', '2026-04-22 10:47:47'),
(7, NULL, 'Изменён статус инцидента #2: in_progress -> resolved', '2026-04-22 10:47:47'),
(8, 2, 'Добавлено событие мониторинга: Тестовое событие', '2026-04-22 11:23:23'),
(9, 2, 'Добавлено событие мониторинга: Подозрительное изменение конфигурации', '2026-04-22 11:26:56'),
(10, 2, 'Добавлено событие мониторинга: Подозрительное изменение конфигурации', '2026-04-22 11:27:19'),
(11, NULL, 'Изменён статус инцидента #1: resolved -> in_progress', '2026-04-22 11:41:01'),
(12, 2, 'Добавлено событие мониторинга: Тестовое событие', '2026-04-22 23:49:53'),
(13, NULL, 'Изменён статус инцидента #1: in_progress -> resolved', '2026-04-22 23:51:37'),
(14, 1, 'Добавлено событие мониторинга: Test', '2026-04-23 00:59:53'),
(15, NULL, 'Изменён статус инцидента #1: resolved -> new', '2026-04-23 01:02:57'),
(16, 1, 'Добавлено событие мониторинга: test', '2026-04-23 22:50:02'),
(17, 1, 'Добавлено событие мониторинга: Test', '2026-04-24 00:11:57'),
(18, NULL, 'Изменён статус инцидента #2: resolved -> in_progress', '2026-04-24 00:12:09');

-- --------------------------------------------------------

--
-- Структура таблицы `resources`
--

CREATE TABLE `resources` (
  `resource_id` int(11) NOT NULL,
  `name` varchar(100) NOT NULL,
  `type` varchar(50) NOT NULL,
  `ip_address` varchar(50) NOT NULL,
  `status` varchar(50) NOT NULL,
  `created_at` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Дамп данных таблицы `resources`
--

INSERT INTO `resources` (`resource_id`, `name`, `type`, `ip_address`, `status`, `created_at`) VALUES
(1, 'SRV-APP-01', 'server', '192.168.10.10', 'active', '2026-04-22 10:47:47'),
(2, 'DB-SEC-01', 'database', '192.168.10.20', 'active', '2026-04-22 10:47:47'),
(3, 'FW-EDGE-01', 'firewall', '192.168.10.1', 'warning', '2026-04-22 10:47:47');

-- --------------------------------------------------------

--
-- Структура таблицы `threats`
--

CREATE TABLE `threats` (
  `threat_id` int(11) NOT NULL,
  `name` varchar(100) NOT NULL,
  `severity` varchar(20) NOT NULL,
  `description` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Дамп данных таблицы `threats`
--

INSERT INTO `threats` (`threat_id`, `name`, `severity`, `description`) VALUES
(1, 'Brute Force', 'high', 'Подбор паролей методом перебора'),
(2, 'Privilege Escalation', 'critical', 'Несанкционированное повышение привилегий'),
(3, 'Reconnaissance', 'medium', 'Разведка и сканирование сетевой инфраструктуры');

-- --------------------------------------------------------

--
-- Структура таблицы `users`
--

CREATE TABLE `users` (
  `user_id` int(11) NOT NULL,
  `login` varchar(100) NOT NULL,
  `password_hash` varchar(64) NOT NULL,
  `password_salt` varchar(32) NOT NULL,
  `full_name` varchar(150) NOT NULL,
  `email` varchar(150) NOT NULL,
  `role_name` varchar(50) NOT NULL,
  `is_active` tinyint(1) NOT NULL DEFAULT 1,
  `created_at` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Дамп данных таблицы `users`
--

INSERT INTO `users` (`user_id`, `login`, `password_hash`, `password_salt`, `full_name`, `email`, `role_name`, `is_active`, `created_at`) VALUES
(1, 'admin', 'f0b7e916a9b71a478a75cb2ccbb5ff84521b91346b34a4bc5769898f6ba3db5a', 'a1b2c3d4e5f60708', 'Соколов Андрей Игоревич', 'admin@isecmonitor.local', 'admin', 1, '2026-04-22 10:47:47'),
(2, 'analyst', 'd4028ac1a168201c0db06169a1ba3619b822fb81e37848617ce2c53bdc8a89e2', 'b2c3d4e5f6070819', 'Петрова Марина Сергеевна', 'analyst@isecmonitor.local', 'analyst', 1, '2026-04-22 10:47:47'),
(3, 'operator', 'ec09ba0be997f7eda6bac1624e418dd11c6b35b900e02c0c6230a63d1f7d13e7', 'c3d4e5f60708192a', 'Иванов Пётр Николаевич', 'operator@isecmonitor.local', 'operator', 1, '2026-04-22 10:47:47'),
(4, 'observer', '170a3f46bc1950ff454fd1b390b048f7e8e2d8188210321b7d1aa7919495106c', 'd4e5f60708192a3b', 'Новак Дмитрий Олегович', 'observer@isecmonitor.local', 'analyst', 1, '2026-04-22 10:47:47');

-- --------------------------------------------------------

--
-- Дублирующая структура для представления `v_events_full`
-- (См. Ниже фактическое представление)
--
CREATE TABLE `v_events_full` (
`event_id` int(11)
,`event_type` varchar(100)
,`event_description` text
,`severity` varchar(20)
,`event_time` datetime
,`resource_id` int(11)
,`resource_name` varchar(100)
,`resource_type` varchar(50)
,`ip_address` varchar(50)
,`user_id` int(11)
,`login` varchar(100)
,`full_name` varchar(150)
,`role_name` varchar(50)
);

-- --------------------------------------------------------

--
-- Дублирующая структура для представления `v_threat_stats`
-- (См. Ниже фактическое представление)
--
CREATE TABLE `v_threat_stats` (
`threat_id` int(11)
,`threat_name` varchar(100)
,`severity` varchar(20)
,`incident_count` bigint(21)
,`new_count` decimal(22,0)
,`in_progress_count` decimal(22,0)
,`resolved_count` decimal(22,0)
);

-- --------------------------------------------------------

--
-- Структура для представления `v_events_full`
--
DROP TABLE IF EXISTS `v_events_full`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_events_full`  AS SELECT `e`.`event_id` AS `event_id`, `e`.`event_type` AS `event_type`, `e`.`event_description` AS `event_description`, `e`.`severity` AS `severity`, `e`.`event_time` AS `event_time`, `r`.`resource_id` AS `resource_id`, `r`.`name` AS `resource_name`, `r`.`type` AS `resource_type`, `r`.`ip_address` AS `ip_address`, `u`.`user_id` AS `user_id`, `u`.`login` AS `login`, `u`.`full_name` AS `full_name`, `u`.`role_name` AS `role_name` FROM ((`events` `e` join `resources` `r` on(`e`.`resource_id` = `r`.`resource_id`)) join `users` `u` on(`e`.`user_id` = `u`.`user_id`)) ;

-- --------------------------------------------------------

--
-- Структура для представления `v_threat_stats`
--
DROP TABLE IF EXISTS `v_threat_stats`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_threat_stats`  AS SELECT `t`.`threat_id` AS `threat_id`, `t`.`name` AS `threat_name`, `t`.`severity` AS `severity`, count(`i`.`incident_id`) AS `incident_count`, sum(case when `i`.`status` = 'new' then 1 else 0 end) AS `new_count`, sum(case when `i`.`status` = 'in_progress' then 1 else 0 end) AS `in_progress_count`, sum(case when `i`.`status` = 'resolved' then 1 else 0 end) AS `resolved_count` FROM (`threats` `t` left join `incidents` `i` on(`t`.`threat_id` = `i`.`threat_id`)) GROUP BY `t`.`threat_id`, `t`.`name`, `t`.`severity` ;

--
-- Индексы сохранённых таблиц
--

--
-- Индексы таблицы `events`
--
ALTER TABLE `events`
  ADD PRIMARY KEY (`event_id`),
  ADD KEY `fk_events_resource` (`resource_id`),
  ADD KEY `fk_events_user` (`user_id`);

--
-- Индексы таблицы `incidents`
--
ALTER TABLE `incidents`
  ADD PRIMARY KEY (`incident_id`),
  ADD KEY `fk_incidents_event` (`event_id`),
  ADD KEY `fk_incidents_threat` (`threat_id`);

--
-- Индексы таблицы `logs`
--
ALTER TABLE `logs`
  ADD PRIMARY KEY (`log_id`),
  ADD KEY `fk_logs_user` (`user_id`);

--
-- Индексы таблицы `resources`
--
ALTER TABLE `resources`
  ADD PRIMARY KEY (`resource_id`);

--
-- Индексы таблицы `threats`
--
ALTER TABLE `threats`
  ADD PRIMARY KEY (`threat_id`);

--
-- Индексы таблицы `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`user_id`),
  ADD UNIQUE KEY `uq_users_login` (`login`),
  ADD UNIQUE KEY `uq_users_email` (`email`);

--
-- AUTO_INCREMENT для сохранённых таблиц
--

--
-- AUTO_INCREMENT для таблицы `events`
--
ALTER TABLE `events`
  MODIFY `event_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT для таблицы `incidents`
--
ALTER TABLE `incidents`
  MODIFY `incident_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT для таблицы `logs`
--
ALTER TABLE `logs`
  MODIFY `log_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=19;

--
-- AUTO_INCREMENT для таблицы `resources`
--
ALTER TABLE `resources`
  MODIFY `resource_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT для таблицы `threats`
--
ALTER TABLE `threats`
  MODIFY `threat_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT для таблицы `users`
--
ALTER TABLE `users`
  MODIFY `user_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- Ограничения внешнего ключа сохраненных таблиц
--

--
-- Ограничения внешнего ключа таблицы `events`
--
ALTER TABLE `events`
  ADD CONSTRAINT `fk_events_resource` FOREIGN KEY (`resource_id`) REFERENCES `resources` (`resource_id`) ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_events_user` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`) ON UPDATE CASCADE;

--
-- Ограничения внешнего ключа таблицы `incidents`
--
ALTER TABLE `incidents`
  ADD CONSTRAINT `fk_incidents_event` FOREIGN KEY (`event_id`) REFERENCES `events` (`event_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_incidents_threat` FOREIGN KEY (`threat_id`) REFERENCES `threats` (`threat_id`) ON UPDATE CASCADE;

--
-- Ограничения внешнего ключа таблицы `logs`
--
ALTER TABLE `logs`
  ADD CONSTRAINT `fk_logs_user` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`) ON DELETE SET NULL ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
