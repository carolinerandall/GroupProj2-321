-- Fix order_items table if subtotal is incorrectly set as GENERATED column
-- If subtotal was created as a GENERATED column, we need to change it to a regular column
-- OR remove it from INSERT statements (which we've done in code)

USE hxcibmkyhpvfozlo;

-- Option 1: If subtotal is a GENERATED column, drop and recreate as regular column
-- First check: DESCRIBE order_items; or SHOW CREATE TABLE order_items;

-- If it's generated, run this to change it:
-- ALTER TABLE order_items 
-- DROP COLUMN subtotal,
-- ADD COLUMN subtotal DECIMAL(10, 2) NOT NULL AS (quantity * unit_price) STORED;

-- Actually, if it's stored as generated, we can just leave it and not insert into it
-- The code has been updated to not insert subtotal values

-- If you want to make it a regular column instead:
ALTER TABLE order_items 
MODIFY COLUMN subtotal DECIMAL(10, 2) NOT NULL;

