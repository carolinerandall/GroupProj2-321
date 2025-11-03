# Remaining Endpoints to Test

## Orders Endpoints:
**Issue Found:** `orders` table missing `created_at` column
**Fix Required:** Run `fix_orders_table.sql` in MySQL
**After Fix, Test:**
- `GET /api/orders/{orderId}` - Get order by ID
- `PUT /api/orders/{orderId}/status` - Update order status
- `POST /api/orders/{orderId}/cancel` - Cancel order
- `POST /api/orders` - Create order

## Payment Endpoints:
- `POST /api/payment` - Create payment
- `GET /api/payment/order/{orderId}` - Get payment by order ID

## Delivery Endpoints:
- `POST /api/deliveries` - Create delivery
- `GET /api/deliveries/order/{orderId}` - Get delivery by order ID
- `PUT /api/deliveries/{deliveryId}/status` - Update delivery status

## Current Status:
✅ All authentication endpoints working
✅ All profile endpoints working
✅ All produce endpoints working
⏳ Orders endpoints - waiting for `created_at` column fix
⏳ Payment endpoints - not yet tested
⏳ Delivery endpoints - not yet tested

