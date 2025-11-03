Overview
FarmToTable connects local farmers with elementary schools to streamline the sale of surplus produce. Farmers can list available produce, while schools browse and order directly from local suppliers. The system tracks itemized orders, delivery logistics (including farmer-delivery vs. third-party delivery), and payments. Each user type — School or Farmer — has a unique login and dashboard with role-specific actions. Data is stored in MySQL using direct SQL queries.
🎯 Project Goals
Build an MVP enabling registration, login, and profile management for both schools and farmers.
Enable farmers to create produce listings with price, availability, and delivery capability.
Enable schools to browse produce, place and manage orders, and track payments.
Support delivery coordination with a 50/50 fee split between schools and farmers.
Use direct MySQL queries for simplicity and transparency.
Deliver a clean, modern Bootstrap UI with green, farm-inspired branding.
🧱 Feature Priorities
Farmer registration and produce management
School registration and order management
Delivery coordination (farmer or external)
Payment tracking and fee splitting
Authentication and session management
⚙️ Tech Stack (Current Implementation)
Frontend: HTML/CSS/JavaScript + Bootstrap 5 (single-page application with div id="app")
Backend: .NET API (ASP.NET Core Web API) + C#
Target Framework: .NET 8.0 or latest LTS version
Database: MySQL with direct SQL queries (no ORM) using MySqlConnector package
Connection String: Stored in appsettings.json
Deployment: Local environment
Auth: Email + password hash stored in MySQL
Payments: Simulated transactions; delivery cost split 50/50
School {
  int school_id (PK)
  string school_name
  string contact_name
  string email (unique)
  string password_hash
  string phone
  string address
  string city
  string state
  string zip_code
  string credit_card_last4
  string credit_card_token
  bool is_verified
  datetime created_at
}

Farmer {
  int farmer_id (PK)
  string farm_name
  string first_name
  string last_name
  string email (unique)
  string password_hash
  string phone
  string address
  string city
  string state
  string zip_code
  string bank_account_last4
  string bank_account_token
  datetime created_at
}

Produce {
  int produce_id (PK)
  int farmer_id (FK → Farmer.farmer_id)
  string produce_name
  string description
  decimal price_per_unit
  int available_quantity
  string unit
  bool farmer_can_deliver
  date availability_start
  date availability_end
  datetime created_at
}

Order {
  int order_id (PK)
  int school_id (FK → School.school_id)
  int farmer_id (FK → Farmer.farmer_id)
  datetime order_date
  datetime delivery_date
  string status (Pending / Confirmed / Shipped / Delivered / Cancelled)
  decimal total_cost
  string payment_status (Unpaid / Paid / Refunded / Partial)
  datetime created_at
}

OrderItem {
  int order_item_id (PK)
  int order_id (FK → Order.order_id)
  int produce_id (FK → Produce.produce_id)
  int quantity
  decimal unit_price
  decimal subtotal
}

Delivery {
  int delivery_id (PK)
  int order_id (FK → Order.order_id)
  string truck_company
  string truck_contact
  decimal delivery_fee_total
  decimal school_share (50%)
  decimal farmer_share (50%)
  string delivery_status (Scheduled / In Transit / Delivered / Cancelled)
  datetime estimated_arrival
}

Payment {
  int payment_id (PK)
  int order_id (FK → Order.order_id)
  decimal amount
  string payment_method
  string transaction_id
  datetime payment_date
  string status (Successful / Pending / Failed / Refunded)
}

SystemLog {
  int log_id (PK)
  string user_email
  string action
  datetime timestamp
  string ip_address
}
Design Guidelines
Theme: Clean, modern, agriculture-focused.
Primary Color: Green (used for headers, buttons, and accent elements).
Secondary Palette: Neutral beige, cream, and light gray backgrounds.
Typography: System fonts with readable body text and bold headings.
Imagery: Farm, produce, and plant photography.
Layout: Bootstrap 5 grid, responsive design, sharp edges.
Buttons: Primary (green) for main actions; secondary outlined buttons for less critical actions.
Accessibility: High-contrast text, labeled form inputs, and mobile responsiveness.
📋 API Endpoints
Authentication
POST /api/auth/register/school — Register a new school
POST /api/auth/register/farmer — Register a new farmer
POST /api/auth/login — Login (email + password)
POST /api/auth/logout — Logout
Schools
GET /api/schools/:id — View school profile
PUT /api/schools/:id — Update school profile
GET /api/schools/:id/orders — List school’s orders
POST /api/schools/orders — Create new order
GET /api/schools/orders/:order_id — View order details
POST /api/schools/orders/:order_id/pay — Record payment
Farmers
GET /api/farmers/:id — View farmer profile
PUT /api/farmers/:id — Update farmer profile
GET /api/farmers/:id/produce — List produce items
POST /api/farmers/:id/produce — Add new produce
PUT /api/produce/:produce_id — Update produce details
Deliveries
POST /api/deliveries — Create or assign delivery
GET /api/deliveries/:order_id — View delivery details
Payments
POST /api/payments — Record payment for an order
GET /api/payments/:order_id — View payment details
🔧 Development Guidelines
Use direct MySQL queries with parameterized statements for safety.
Maintain a clear folder structure:
/Controllers for API endpoint logic and SQL calls
/Services for business logic and data access helpers
/Models for data models (if needed)
Include inline comments for all database queries.
Use sessions or JWT tokens for authentication.
Keep endpoints RESTful and consistent with proper HTTP methods and status codes.
Ensure order creation triggers inserts into order_items, deliveries, and payments as needed.
Test full CRUD flow for both schools and farmers.
Follow .NET naming conventions: PascalCase for classes, camelCase for variables.