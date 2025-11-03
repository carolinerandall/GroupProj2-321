-- Fix orders table - add missing created_at column
USE hxcibmkyhpvfozlo;

ALTER TABLE orders ADD COLUMN created_at DATETIME DEFAULT CURRENT_TIMESTAMP;

