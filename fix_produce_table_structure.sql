-- Fix produce table structure without dropping the table
-- This script adds missing columns if they don't exist

-- Check and add farmer_can_deliver column if missing
-- Note: MySQL doesn't support IF NOT EXISTS for columns directly in all versions
-- So we'll use ALTER TABLE with a check, or just ALTER and ignore if exists

-- First, let's see what columns exist and fix them
-- If farmer_can_deliver doesn't exist, add it
ALTER TABLE produce 
ADD COLUMN IF NOT EXISTS farmer_can_deliver BOOLEAN DEFAULT FALSE;

-- If the above doesn't work (older MySQL versions), use this alternative:
-- We'll need to check manually or use a stored procedure, but for simplicity:
-- Try to add the column, and ignore error if it already exists

-- Alternative approach: Check table structure and add only what's missing
-- Since we can't easily check, we'll use a safer method:

-- Method 1: Try adding column, ignore error
-- (MySQL Workbench will show error but won't break if column exists)

-- Method 2: Use a stored procedure to check and add
DELIMITER $$

CREATE PROCEDURE IF NOT EXISTS FixProduceTable()
BEGIN
    -- Check if farmer_can_deliver exists
    IF NOT EXISTS (
        SELECT * FROM information_schema.COLUMNS 
        WHERE TABLE_SCHEMA = DATABASE() 
        AND TABLE_NAME = 'produce' 
        AND COLUMN_NAME = 'farmer_can_deliver'
    ) THEN
        ALTER TABLE produce ADD COLUMN farmer_can_deliver BOOLEAN DEFAULT FALSE;
    END IF;
    
    -- Check other columns too
    IF NOT EXISTS (
        SELECT * FROM information_schema.COLUMNS 
        WHERE TABLE_SCHEMA = DATABASE() 
        AND TABLE_NAME = 'produce' 
        AND COLUMN_NAME = 'description'
    ) THEN
        ALTER TABLE produce ADD COLUMN description TEXT;
    END IF;
END$$

DELIMITER ;

-- Execute the procedure
CALL FixProduceTable();

-- Drop the procedure after use
DROP PROCEDURE IF EXISTS FixProduceTable;

