-- Fix for "Table 'mysql.produce' doesn't exist" error
-- You need to select the correct database first!

-- Step 1: Select your database
-- (From your connection string in appsettings.json, the database is: hxcibmkyhpvfozlo)
USE hxcibmkyhpvfozlo;

-- Step 2: Verify you're in the right database (optional check)
SELECT DATABASE();

-- Step 3: Check if produce table exists in this database
SHOW TABLES LIKE 'produce';

-- Step 4: If table exists, add the missing column
ALTER TABLE produce ADD COLUMN farmer_can_deliver BOOLEAN DEFAULT FALSE;

-- Step 5: Verify the column was added
DESCRIBE produce;

