#!/bin/bash

API_BASE="http://localhost:5000/api"
echo "=== Testing FarmToTable API ===\n"

echo "1. Testing Farmer Sign Up..."
FARMER_SIGNUP=$(curl -s -X POST $API_BASE/users/farmer \
  -H "Content-Type: application/json" \
  -d '{
    "email": "testfarmer@test.com",
    "password": "testpass123",
    "farmName": "Test Farm",
    "firstName": "John",
    "lastName": "Farmer",
    "phone": "555-0100",
    "address": "123 Farm Road",
    "city": "Farmville",
    "state": "CA",
    "zipCode": "12345"
  }')
echo "Response: $FARMER_SIGNUP"
echo ""

echo "2. Testing Farmer Login..."
FARMER_LOGIN=$(curl -s -X POST $API_BASE/auth/farmer \
  -H "Content-Type: application/json" \
  -d '{
    "email": "testfarmer@test.com",
    "password": "testpass123"
  }')
echo "Response: $FARMER_LOGIN"
FARMER_ID=$(echo $FARMER_LOGIN | grep -o '"userId":[0-9]*' | cut -d':' -f2)
echo "Farmer ID: $FARMER_ID"
echo ""

if [ -z "$FARMER_ID" ]; then
    echo "ERROR: Could not get farmer ID from login. Stopping tests."
    exit 1
fi

echo "3. Testing Get Farmer Profile..."
curl -s $API_BASE/farmer/profile/$FARMER_ID
echo "\n"

echo "4. Testing Update Farmer Profile..."
curl -s -X PUT $API_BASE/farmer/profile/$FARMER_ID \
  -H "Content-Type: application/json" \
  -d '{
    "farmName": "Updated Test Farm",
    "phone": "555-9999"
  }'
echo "\n"

echo "5. Testing Get Farmer Produce (should be empty)..."
curl -s $API_BASE/farmer/$FARMER_ID/produce
echo "\n"

echo "6. Testing Add Produce..."
PRODUCE_ADD=$(curl -s -X POST $API_BASE/farmer/$FARMER_ID/produce \
  -H "Content-Type: application/json" \
  -d '{
    "produceName": "Organic Tomatoes",
    "description": "Fresh organic tomatoes",
    "pricePerUnit": 5.50,
    "availableQuantity": 100,
    "unit": "lb",
    "farmerCanDeliver": true,
    "availabilityStart": "2024-01-01",
    "availabilityEnd": "2024-12-31"
  }')
echo "Response: $PRODUCE_ADD"
echo ""

echo "7. Testing Get Farmer Produce (should have 1 item)..."
curl -s $API_BASE/farmer/$FARMER_ID/produce
echo "\n"

echo "8. Testing Get Farmer Orders (should be empty)..."
curl -s $API_BASE/farmer/$FARMER_ID/orders
echo "\n"

echo "\n=== Testing School Functionality ===\n"

echo "9. Testing School Sign Up..."
SCHOOL_SIGNUP=$(curl -s -X POST $API_BASE/users/school \
  -H "Content-Type: application/json" \
  -d '{
    "email": "testschool@test.com",
    "password": "testpass123",
    "schoolName": "Test Elementary School",
    "contactName": "Jane Principal",
    "phone": "555-0200",
    "address": "456 School Street",
    "city": "Schooltown",
    "state": "CA",
    "zipCode": "54321"
  }')
echo "Response: $SCHOOL_SIGNUP"
echo ""

echo "10. Testing School Login..."
SCHOOL_LOGIN=$(curl -s -X POST $API_BASE/auth/school \
  -H "Content-Type: application/json" \
  -d '{
    "email": "testschool@test.com",
    "password": "testpass123"
  }')
echo "Response: $SCHOOL_LOGIN"
SCHOOL_ID=$(echo $SCHOOL_LOGIN | grep -o '"userId":[0-9]*' | cut -d':' -f2)
echo "School ID: $SCHOOL_ID"
echo ""

if [ -z "$SCHOOL_ID" ]; then
    echo "ERROR: Could not get school ID from login. Stopping tests."
    exit 1
fi

echo "11. Testing Get School Profile..."
curl -s $API_BASE/school/profile/$SCHOOL_ID
echo "\n"

echo "12. Testing Update School Profile..."
curl -s -X PUT $API_BASE/school/profile/$SCHOOL_ID \
  -H "Content-Type: application/json" \
  -d '{
    "schoolName": "Updated Test School",
    "contactName": "Updated Contact"
  }'
echo "\n"

echo "13. Testing Get Available Produce..."
curl -s $API_BASE/school/produce/available
echo "\n"

echo "14. Testing Get School Orders (should be empty)..."
curl -s $API_BASE/school/$SCHOOL_ID/orders
echo "\n"

echo "\n=== Test Summary ==="
echo "Farmer ID: $FARMER_ID"
echo "School ID: $SCHOOL_ID"

