# Issues Found During Testing

## Testing Date: November 3, 2025

### ✅ WORKING Endpoints:

#### Farmer Authentication:
- ✅ `POST /api/users/farmer` - Farmer Sign Up (HTTP 200)
- ✅ `POST /api/auth/farmer` - Farmer Login (HTTP 200)
- ✅ `GET /api/farmer/profile/{farmerId}` - Get Farmer Profile (HTTP 200)

#### School Authentication:
- ✅ `POST /api/users/school` - School Sign Up (HTTP 200)
- ✅ `POST /api/auth/school` - School Login (HTTP 200)

### ✅ ADDITIONAL WORKING Endpoints:
- ✅ `PUT /api/farmer/profile/{farmerId}` - Update Farmer Profile (HTTP 200)
- ✅ `PUT /api/school/profile/{schoolId}` - Update School Profile (HTTP 200)

### ❌ ENDPOINTS WITH ERRORS:

#### Farmer Produce Endpoints:
- ❌ `POST /api/farmer/{farmerId}/produce` - Returns HTTP 500 "An error occurred while adding produce"
  - **Issue**: Error when trying to add produce
  - **Tested with**: Different date formats (ISO strings, simple dates) - all fail
  - **Possible Causes**: 
    - Date format conversion issue (table uses DATE, code sends DateTime)
    - Foreign key constraint issue (farmer_id reference)
    - Table structure mismatch (BOOLEAN vs TINYINT in MySQL)
    - Missing index or constraint
  - **Need to investigate**: Check actual MySQL error logs or add better error logging

- ❌ `GET /api/farmer/{farmerId}/produce` - Returns HTTP 500 "An error occurred while retrieving produce"
  - **Issue**: Error when retrieving produce list (even if empty)
  - **Note**: Same table as add produce, likely same root cause
  - **Possible**: Query syntax issue or table doesn't exist despite SQL script execution

- ❌ `PUT /api/farmer/produce/{produceId}` - Not tested yet (depends on add produce working)

#### Order Endpoints:
- ❌ `GET /api/farmer/{farmerId}/orders` - Returns HTTP 500 "An error occurred while retrieving orders"
  - **Issue**: Error retrieving farmer orders
  - **Possible Causes**: Table structure issue or query problem

- ❌ `GET /api/school/{schoolId}/orders` - Returns HTTP 500 "An error occurred while retrieving orders"
  - **Issue**: Error retrieving school orders

- ❌ `POST /api/orders` - Not tested yet (depends on produce working)

#### School Browse Produce:
- ❌ `GET /api/school/produce/available` - Returns HTTP 500 "An error occurred while retrieving available produce"
  - **Issue**: Error when schools try to browse available produce
  - **Note**: Same table (produce) as farmer produce endpoints

### 📊 Summary:

**Tables Confirmed Working:**
- ✅ `farmers` table - All operations work
- ✅ `schools` table - All operations work

**Tables with Issues:**
- ❌ `produce` table - Create and Read operations failing (HTTP 500)
- ❌ `orders` table - Read operations failing (HTTP 500)

**Not Yet Tested:**
- Update Farmer Profile (PUT)
- Update School Profile (PUT)
- Create Orders
- Update Produce
- Payment endpoints
- Delivery endpoints

### Investigation Needed:
The produce table errors are consistent across all produce-related operations. Possible issues:
1. **Date Type Mismatch**: Table uses `DATE` but code may be sending `DATETIME` - MySQL might be strict about this
2. **BOOLEAN vs TINYINT**: MySQL may have created the table with TINYINT instead of BOOLEAN
3. **Foreign Key Constraint**: The `farmer_id` foreign key might be failing
4. **Table Creation**: Despite SQL execution, table might not have been created correctly

### Next Steps:
1. **HIGH PRIORITY**: Fix produce table errors - this blocks all produce functionality
   - Check actual MySQL error by improving error logging or querying database directly
   - Verify table structure matches expected schema
   - Fix any data type mismatches
2. Test order creation endpoints (depends on produce working)
3. Test payment endpoints
4. Test delivery endpoints
5. Complete systematic testing of all remaining endpoints

