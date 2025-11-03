-- Simple fix for produce table - add missing column
-- Just run this one line. If you get an error that the column already exists, that's fine - it means it's already there.

ALTER TABLE produce ADD COLUMN farmer_can_deliver BOOLEAN DEFAULT FALSE;

