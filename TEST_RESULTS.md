# FarmToTable API Test Results

## Test Date: November 3, 2025

### Critical Issues Found:

#### 1. **API ENDPOINTS RETURNING 404 NOT FOUND** ⚠️ CRITICAL
   - **Issue**: All API endpoints tested return HTTP 404 Not Found
   - **Endpoints Tested:**
     - `POST /api/users/farmer` - 404
     - `POST /api/auth/farmer` - 404
     - `POST /api/users/school` - 404
     - `POST /api/auth/school` - 404
     - `GET /api/farmer/profile/{id}` - 404
     - `GET /api/school/profile/{id}` - 404
   
   - **Root Cause**: Likely middleware configuration issue in `Program.cs`
     - `UseDefaultFiles()` and `UseStaticFiles()` with `RequestPath = ""` are configured to serve from the Client folder
     - This configuration may be intercepting ALL requests (including `/api/*` routes) before they reach `MapControllers()`
     - Static file middleware should either:
       a) Come AFTER `UseRouting()` and `MapControllers()`, OR
       b) Use a specific `RequestPath` that doesn't match `/api/*`
   
   - **Impact**: Cannot test any functionality until API routing is fixed

### Tests That Could NOT Be Completed Due to 404 Errors:

#### Farmer Functionality:
- ❌ Farmer Sign Up (`POST /api/users/farmer`)
- ❌ Farmer Login (`POST /api/auth/farmer`)
- ❌ Get Farmer Profile (`GET /api/farmer/profile/{farmerId}`)
- ❌ Update Farmer Profile (`PUT /api/farmer/profile/{farmerId}`)
- ❌ Get Farmer Produce (`GET /api/farmer/{farmerId}/produce`)
- ❌ Add Produce (`POST /api/farmer/{farmerId}/produce`)
- ❌ Update Produce (`PUT /api/farmer/produce/{produceId}`)
- ❌ Get Farmer Orders (`GET /api/farmer/{farmerId}/orders`)

#### School Functionality:
- ❌ School Sign Up (`POST /api/users/school`)
- ❌ School Login (`POST /api/auth/school`)
- ❌ Get School Profile (`GET /api/school/profile/{schoolId}`)
- ❌ Update School Profile (`PUT /api/school/profile/{schoolId}`)
- ❌ Get Available Produce (`GET /api/school/produce/available`)
- ❌ Get School Orders (`GET /api/school/{schoolId}/orders`)

#### Order Functionality:
- ❌ Create Order (`POST /api/orders`)
- ❌ Get Order (`GET /api/orders/{orderId}`)
- ❌ Update Order Status (`PUT /api/orders/{orderId}/status`)
- ❌ Cancel Order (`POST /api/orders/{orderId}/cancel`)

#### Payment Functionality:
- ❌ Create Payment (`POST /api/payment`)
- ❌ Get Payment by Order (`GET /api/payment/order/{orderId}`)

#### Delivery Functionality:
- ❌ Create Delivery (`POST /api/deliveries`)
- ❌ Get Delivery by Order (`GET /api/deliveries/order/{orderId}`)
- ❌ Update Delivery Status (`PUT /api/deliveries/{deliveryId}/status`)

### What Was Tested:
- ✅ Server starts successfully
- ✅ Server responds to requests (returns 404, not connection refused)
- ✅ Build completes without errors
- ✅ Controllers are defined with correct routes

### Additional Notes:
- Database tables may not exist yet (would cause different errors, not 404s)
- Connection string is configured in `appsettings.json`
- All controller routes appear to be correctly defined
- Middleware order in `Program.cs` needs adjustment

### Recommended Fix Priority:
1. **HIGH**: Fix middleware order in `Program.cs` to allow API routes to work
2. **HIGH**: Verify database tables exist (create if needed)
3. **MEDIUM**: Test all endpoints after routing fix
4. **MEDIUM**: Add error handling and logging for better debugging

