-- Simple script to add the missing farmer_can_deliver column
-- Run this script. If you get an error saying the column already exists, that's okay - it means the column is there.

ALTER TABLE produce 
ADD COLUMN farmer_can_deliver BOOLEAN DEFAULT FALSE;

-- If the above gives an error "Duplicate column name 'farmer_can_deliver'", 
-- that means the column already exists and you can ignore the error.
-- The issue might be something else - check the table structure with:
-- DESCRIBE produce;

