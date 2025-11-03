-- Simple fix: Add missing farmer_can_deliver column to produce table
-- IMPORTANT: First make sure you're using the correct database!

-- Select your database (the one from your connection string)
USE hxcibmkyhpvfozlo;

-- For MySQL 8.0.19+ (check your version first):
-- ALTER TABLE produce ADD COLUMN IF NOT EXISTS farmer_can_deliver BOOLEAN DEFAULT FALSE;

-- For older MySQL versions (or if IF NOT EXISTS doesn't work):
-- Just run this line. If you get "Duplicate column name" error, the column already exists.
ALTER TABLE produce ADD COLUMN farmer_can_deliver BOOLEAN DEFAULT FALSE;

-- To check if it worked, run:
-- DESCRIBE produce;
-- You should see farmer_can_deliver in the list

