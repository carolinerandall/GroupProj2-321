# Complete Database Schema Review

## Current Status Analysis

### What We Know Works:
✅ **farmers table** - EXISTS (signup/login works)
✅ **schools table** - EXISTS (signup/login works)

### What Has Issues:
❌ **produce table** - MISSING `farmer_can_deliver` column

### What Tables Should Exist (from Models/Code):

#### 1. **farmers** table needs:
- farmer_id (PK, AUTO_INCREMENT)
- farm_name
- first_name
- last_name
- email (unique)
- password_hash
- phone (nullable)
- address (nullable)
- city (nullable)
- state (nullable)
- zip_code (nullable)
- bank_account_last4 (nullable) - for payment info
- bank_account_token (nullable) - for payment info
- created_at (DATETIME)

#### 2. **schools** table needs:
- school_id (PK, AUTO_INCREMENT)
- school_name
- contact_name
- email (unique)
- password_hash
- phone (nullable)
- address (nullable)
- city (nullable)
- state (nullable)
- zip_code (nullable)
- credit_card_last4 (nullable) - for payment info
- credit_card_token (nullable) - for payment info
- is_verified (BOOLEAN, default false)
- created_at (DATETIME)

#### 3. **produce** table needs:
- produce_id (PK, AUTO_INCREMENT)
- farmer_id (FK to farmers)
- produce_name
- description (nullable, TEXT)
- price_per_unit (DECIMAL)
- available_quantity (INT, default 0)
- unit (VARCHAR)
- farmer_can_deliver (BOOLEAN) ← **MISSING THIS**
- availability_start (DATE)
- availability_end (DATE)
- created_at (DATETIME)

#### 4. **orders** table (exists in script)
#### 5. **order_items** table (exists in script)
#### 6. **deliveries** table (exists in script)
#### 7. **payments** table (exists in script)
#### 8. **system_logs** table (exists in script, optional)

## Problem Identified:
The `create_tables.sql` file is **INCOMPLETE** - it doesn't create the `farmers` and `schools` tables!

These tables must exist already (since signup works), but we need to:
1. Verify they have ALL required columns
2. Add them to create_tables.sql for future reference
3. Fix the produce table missing column

