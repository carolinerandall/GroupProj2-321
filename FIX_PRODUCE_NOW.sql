-- SIMPLE FIX - Make sure you're using the correct database!
-- First, select your database (replace with your actual database name if different)
-- From appsettings.json, the database name is: hxcibmkyhpvfozlo

USE hxcibmkyhpvfozlo;

-- Then run this ALTER statement
ALTER TABLE produce ADD COLUMN farmer_can_deliver BOOLEAN DEFAULT FALSE;

-- If you get an error saying "Duplicate column name 'farmer_can_deliver'", 
-- that means the column already exists. In that case, check what columns you DO have:
-- 
-- DESCRIBE produce;
--
-- Then we can figure out what's actually missing.

