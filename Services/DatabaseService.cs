using MySqlConnector;
using GroupProj2_321.Models;
using GroupProj2_321.Controllers;

namespace GroupProj2_321.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException("Connection string not found");
        }

        /// <summary>
        /// Executes a query and returns a MySqlConnection for manual query execution
        /// </summary>
        public async Task<MySqlConnection> GetConnectionAsync()
        {
            var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        /// <summary>
        /// Tests the database connection
        /// </summary>
        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // ==================== FARMER METHODS ====================

        /// <summary>
        /// Authenticates a farmer by email and password
        /// </summary>
        public async Task<Farmer?> AuthenticateFarmerAsync(string email, string passwordHash)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                SELECT farmer_id, farm_name, first_name, last_name, email, password_hash, phone, 
                       address, city, state, zip_code, bank_account_last4, bank_account_token, created_at
                FROM farmers
                WHERE email = @email AND password_hash = @passwordHash";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@passwordHash", passwordHash);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Farmer
                {
                    FarmerId = reader.GetInt32("farmer_id"),
                    FarmName = reader.GetString("farm_name"),
                    FirstName = reader.GetString("first_name"),
                    LastName = reader.GetString("last_name"),
                    Email = reader.GetString("email"),
                    PasswordHash = reader.GetString("password_hash"),
                    Phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString("phone"),
                    Address = reader.IsDBNull(reader.GetOrdinal("address")) ? null : reader.GetString("address"),
                    City = reader.IsDBNull(reader.GetOrdinal("city")) ? null : reader.GetString("city"),
                    State = reader.IsDBNull(reader.GetOrdinal("state")) ? null : reader.GetString("state"),
                    ZipCode = reader.IsDBNull(reader.GetOrdinal("zip_code")) ? null : reader.GetString("zip_code"),
                    BankAccountLast4 = reader.IsDBNull(reader.GetOrdinal("bank_account_last4")) ? null : reader.GetString("bank_account_last4"),
                    BankAccountToken = reader.IsDBNull(reader.GetOrdinal("bank_account_token")) ? null : reader.GetString("bank_account_token"),
                    CreatedAt = reader.GetDateTime("created_at")
                };
            }

            return null;
        }

        /// <summary>
        /// Checks if a farmer email already exists
        /// </summary>
        public async Task<bool> FarmerEmailExistsAsync(string email)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT COUNT(*) FROM farmers WHERE email = @email";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@email", email);

            var count = await command.ExecuteScalarAsync();
            return Convert.ToInt32(count) > 0;
        }

        /// <summary>
        /// Creates a new farmer account
        /// </summary>
        public async Task<int> CreateFarmerAsync(string farmName, string firstName, string lastName, 
            string email, string passwordHash, string? phone, string? address, string? city, 
            string? state, string? zipCode)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                INSERT INTO farmers (farm_name, first_name, last_name, email, password_hash, phone, 
                                    address, city, state, zip_code, created_at)
                VALUES (@farmName, @firstName, @lastName, @email, @passwordHash, @phone, 
                       @address, @city, @state, @zipCode, NOW())";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@farmName", farmName);
            command.Parameters.AddWithValue("@firstName", firstName);
            command.Parameters.AddWithValue("@lastName", lastName);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@passwordHash", passwordHash);
            command.Parameters.AddWithValue("@phone", phone ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@address", address ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@city", city ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@state", state ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@zipCode", zipCode ?? (object)DBNull.Value);

            await command.ExecuteNonQueryAsync();
            return (int)command.LastInsertedId;
        }

        /// <summary>
        /// Gets farmer profile by farmer ID
        /// </summary>
        public async Task<Farmer?> GetFarmerProfileAsync(int farmerId)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                SELECT farmer_id, farm_name, first_name, last_name, email, password_hash, phone, 
                       address, city, state, zip_code, bank_account_last4, bank_account_token, created_at
                FROM farmers
                WHERE farmer_id = @farmerId";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@farmerId", farmerId);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Farmer
                {
                    FarmerId = reader.GetInt32("farmer_id"),
                    FarmName = reader.GetString("farm_name"),
                    FirstName = reader.GetString("first_name"),
                    LastName = reader.GetString("last_name"),
                    Email = reader.GetString("email"),
                    PasswordHash = reader.GetString("password_hash"),
                    Phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString("phone"),
                    Address = reader.IsDBNull(reader.GetOrdinal("address")) ? null : reader.GetString("address"),
                    City = reader.IsDBNull(reader.GetOrdinal("city")) ? null : reader.GetString("city"),
                    State = reader.IsDBNull(reader.GetOrdinal("state")) ? null : reader.GetString("state"),
                    ZipCode = reader.IsDBNull(reader.GetOrdinal("zip_code")) ? null : reader.GetString("zip_code"),
                    BankAccountLast4 = reader.IsDBNull(reader.GetOrdinal("bank_account_last4")) ? null : reader.GetString("bank_account_last4"),
                    BankAccountToken = reader.IsDBNull(reader.GetOrdinal("bank_account_token")) ? null : reader.GetString("bank_account_token"),
                    CreatedAt = reader.GetDateTime("created_at")
                };
            }

            return null;
        }

        /// <summary>
        /// Updates farmer profile
        /// </summary>
        public async Task UpdateFarmerProfileAsync(int farmerId, string? farmName, string? firstName, 
            string? lastName, string? phone, string? address, string? city, string? state, string? zipCode)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                UPDATE farmers 
                SET farm_name = COALESCE(@farmName, farm_name),
                    first_name = COALESCE(@firstName, first_name),
                    last_name = COALESCE(@lastName, last_name),
                    phone = @phone,
                    address = @address,
                    city = @city,
                    state = @state,
                    zip_code = @zipCode
                WHERE farmer_id = @farmerId";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@farmerId", farmerId);
            command.Parameters.AddWithValue("@farmName", farmName ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@firstName", firstName ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@lastName", lastName ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@phone", phone ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@address", address ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@city", city ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@state", state ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@zipCode", zipCode ?? (object)DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Gets all produce items for a farmer
        /// </summary>
        public async Task<List<Produce>> GetFarmerProduceAsync(int farmerId)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                SELECT produce_id, farmer_id, produce_name, description, price_per_unit, 
                       available_quantity, unit, farmer_can_deliver, availability_start, 
                       availability_end, created_at
                FROM produce
                WHERE farmer_id = @farmerId
                ORDER BY created_at DESC";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@farmerId", farmerId);

            var produceList = new List<Produce>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                produceList.Add(new Produce
                {
                    ProduceId = reader.GetInt32("produce_id"),
                    FarmerId = reader.GetInt32("farmer_id"),
                    ProduceName = reader.GetString("produce_name"),
                    Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString("description"),
                    PricePerUnit = reader.GetDecimal("price_per_unit"),
                    AvailableQuantity = reader.GetInt32("available_quantity"),
                    Unit = reader.GetString("unit"),
                    FarmerCanDeliver = reader.GetBoolean("farmer_can_deliver"),
                    AvailabilityStart = reader.GetDateTime("availability_start"),
                    AvailabilityEnd = reader.GetDateTime("availability_end"),
                    CreatedAt = reader.GetDateTime("created_at")
                });
            }

            return produceList;
        }

        /// <summary>
        /// Gets farmer's orders
        /// </summary>
        public async Task<List<Order>> GetFarmerOrdersAsync(int farmerId)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                SELECT order_id, school_id, farmer_id, order_date, delivery_date, status, 
                       total_cost, payment_status, created_at
                FROM orders
                WHERE farmer_id = @farmerId
                ORDER BY order_date DESC";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@farmerId", farmerId);

            var orders = new List<Order>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                orders.Add(new Order
                {
                    OrderId = reader.GetInt32("order_id"),
                    SchoolId = reader.GetInt32("school_id"),
                    FarmerId = reader.GetInt32("farmer_id"),
                    OrderDate = reader.GetDateTime("order_date"),
                    DeliveryDate = reader.GetDateTime("delivery_date"),
                    Status = reader.GetString("status"),
                    TotalCost = reader.GetDecimal("total_cost"),
                    PaymentStatus = reader.GetString("payment_status"),
                    CreatedAt = reader.GetDateTime("created_at")
                });
            }

            return orders;
        }

        // ==================== SCHOOL METHODS ====================

        /// <summary>
        /// Authenticates a school by email and password
        /// </summary>
        public async Task<School?> AuthenticateSchoolAsync(string email, string passwordHash)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                SELECT school_id, school_name, contact_name, email, password_hash, phone, 
                       address, city, state, zip_code, credit_card_last4, credit_card_token, 
                       is_verified, created_at
                FROM schools
                WHERE email = @email AND password_hash = @passwordHash";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@passwordHash", passwordHash);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new School
                {
                    SchoolId = reader.GetInt32("school_id"),
                    SchoolName = reader.GetString("school_name"),
                    ContactName = reader.GetString("contact_name"),
                    Email = reader.GetString("email"),
                    PasswordHash = reader.GetString("password_hash"),
                    Phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString("phone"),
                    Address = reader.IsDBNull(reader.GetOrdinal("address")) ? null : reader.GetString("address"),
                    City = reader.IsDBNull(reader.GetOrdinal("city")) ? null : reader.GetString("city"),
                    State = reader.IsDBNull(reader.GetOrdinal("state")) ? null : reader.GetString("state"),
                    ZipCode = reader.IsDBNull(reader.GetOrdinal("zip_code")) ? null : reader.GetString("zip_code"),
                    CreditCardLast4 = reader.IsDBNull(reader.GetOrdinal("credit_card_last4")) ? null : reader.GetString("credit_card_last4"),
                    CreditCardToken = reader.IsDBNull(reader.GetOrdinal("credit_card_token")) ? null : reader.GetString("credit_card_token"),
                    IsVerified = reader.GetBoolean("is_verified"),
                    CreatedAt = reader.GetDateTime("created_at")
                };
            }

            return null;
        }

        /// <summary>
        /// Checks if a school email already exists
        /// </summary>
        public async Task<bool> SchoolEmailExistsAsync(string email)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT COUNT(*) FROM schools WHERE email = @email";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@email", email);

            var count = await command.ExecuteScalarAsync();
            return Convert.ToInt32(count) > 0;
        }

        /// <summary>
        /// Creates a new school account
        /// </summary>
        public async Task<int> CreateSchoolAsync(string schoolName, string contactName, string email, 
            string passwordHash, string? phone, string? address, string? city, string? state, string? zipCode)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                INSERT INTO schools (school_name, contact_name, email, password_hash, phone, 
                                   address, city, state, zip_code, is_verified, created_at)
                VALUES (@schoolName, @contactName, @email, @passwordHash, @phone, 
                       @address, @city, @state, @zipCode, FALSE, NOW())";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@schoolName", schoolName);
            command.Parameters.AddWithValue("@contactName", contactName);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@passwordHash", passwordHash);
            command.Parameters.AddWithValue("@phone", phone ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@address", address ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@city", city ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@state", state ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@zipCode", zipCode ?? (object)DBNull.Value);

            await command.ExecuteNonQueryAsync();
            return (int)command.LastInsertedId;
        }

        /// <summary>
        /// Gets school profile by school ID
        /// </summary>
        public async Task<School?> GetSchoolProfileAsync(int schoolId)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                SELECT school_id, school_name, contact_name, email, password_hash, phone, 
                       address, city, state, zip_code, credit_card_last4, credit_card_token, 
                       is_verified, created_at
                FROM schools
                WHERE school_id = @schoolId";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@schoolId", schoolId);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new School
                {
                    SchoolId = reader.GetInt32("school_id"),
                    SchoolName = reader.GetString("school_name"),
                    ContactName = reader.GetString("contact_name"),
                    Email = reader.GetString("email"),
                    PasswordHash = reader.GetString("password_hash"),
                    Phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString("phone"),
                    Address = reader.IsDBNull(reader.GetOrdinal("address")) ? null : reader.GetString("address"),
                    City = reader.IsDBNull(reader.GetOrdinal("city")) ? null : reader.GetString("city"),
                    State = reader.IsDBNull(reader.GetOrdinal("state")) ? null : reader.GetString("state"),
                    ZipCode = reader.IsDBNull(reader.GetOrdinal("zip_code")) ? null : reader.GetString("zip_code"),
                    CreditCardLast4 = reader.IsDBNull(reader.GetOrdinal("credit_card_last4")) ? null : reader.GetString("credit_card_last4"),
                    CreditCardToken = reader.IsDBNull(reader.GetOrdinal("credit_card_token")) ? null : reader.GetString("credit_card_token"),
                    IsVerified = reader.GetBoolean("is_verified"),
                    CreatedAt = reader.GetDateTime("created_at")
                };
            }

            return null;
        }

        /// <summary>
        /// Updates school profile
        /// </summary>
        public async Task UpdateSchoolProfileAsync(int schoolId, string? schoolName, string? contactName,
            string? phone, string? address, string? city, string? state, string? zipCode)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                UPDATE schools 
                SET school_name = COALESCE(@schoolName, school_name),
                    contact_name = COALESCE(@contactName, contact_name),
                    phone = @phone,
                    address = @address,
                    city = @city,
                    state = @state,
                    zip_code = @zipCode
                WHERE school_id = @schoolId";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@schoolId", schoolId);
            command.Parameters.AddWithValue("@schoolName", schoolName ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@contactName", contactName ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@phone", phone ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@address", address ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@city", city ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@state", state ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@zipCode", zipCode ?? (object)DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Gets available produce items for browsing
        /// </summary>
        public async Task<List<Produce>> GetAvailableProduceAsync(string? produceName = null, int? farmerId = null)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                SELECT p.produce_id, p.farmer_id, p.produce_name, p.description, p.price_per_unit, 
                       p.available_quantity, p.unit, p.farmer_can_deliver, p.availability_start, 
                       p.availability_end, p.created_at,
                       f.farm_name, f.first_name, f.last_name
                FROM produce p
                JOIN farmers f ON p.farmer_id = f.farmer_id
                WHERE p.available_quantity > 0 
                AND p.availability_start <= CURDATE() 
                AND p.availability_end >= CURDATE()";

            if (!string.IsNullOrWhiteSpace(produceName))
            {
                query += " AND p.produce_name LIKE @produceName";
            }

            if (farmerId.HasValue)
            {
                query += " AND p.farmer_id = @farmerId";
            }

            query += " ORDER BY p.created_at DESC";

            using var command = new MySqlCommand(query, connection);
            if (!string.IsNullOrWhiteSpace(produceName))
            {
                command.Parameters.AddWithValue("@produceName", $"%{produceName}%");
            }
            if (farmerId.HasValue)
            {
                command.Parameters.AddWithValue("@farmerId", farmerId.Value);
            }

            var produceList = new List<Produce>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                produceList.Add(new Produce
                {
                    ProduceId = reader.GetInt32("produce_id"),
                    FarmerId = reader.GetInt32("farmer_id"),
                    ProduceName = reader.GetString("produce_name"),
                    Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString("description"),
                    PricePerUnit = reader.GetDecimal("price_per_unit"),
                    AvailableQuantity = reader.GetInt32("available_quantity"),
                    Unit = reader.GetString("unit"),
                    FarmerCanDeliver = reader.GetBoolean("farmer_can_deliver"),
                    AvailabilityStart = reader.GetDateTime("availability_start"),
                    AvailabilityEnd = reader.GetDateTime("availability_end"),
                    CreatedAt = reader.GetDateTime("created_at")
                });
            }

            return produceList;
        }

        /// <summary>
        /// Gets school's orders
        /// </summary>
        public async Task<List<Order>> GetSchoolOrdersAsync(int schoolId)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                SELECT order_id, school_id, farmer_id, order_date, delivery_date, status, 
                       total_cost, payment_status, created_at
                FROM orders
                WHERE school_id = @schoolId
                ORDER BY order_date DESC";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@schoolId", schoolId);

            var orders = new List<Order>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                orders.Add(new Order
                {
                    OrderId = reader.GetInt32("order_id"),
                    SchoolId = reader.GetInt32("school_id"),
                    FarmerId = reader.GetInt32("farmer_id"),
                    OrderDate = reader.GetDateTime("order_date"),
                    DeliveryDate = reader.GetDateTime("delivery_date"),
                    Status = reader.GetString("status"),
                    TotalCost = reader.GetDecimal("total_cost"),
                    PaymentStatus = reader.GetString("payment_status"),
                    CreatedAt = reader.GetDateTime("created_at")
                });
            }

            return orders;
        }

        // ==================== PRODUCE METHODS ====================

        /// <summary>
        /// Adds a new produce item
        /// </summary>
        public async Task<int> AddProduceAsync(int farmerId, string produceName, string? description,
            decimal pricePerUnit, int availableQuantity, string unit, bool farmerCanDeliver,
            DateTime availabilityStart, DateTime availabilityEnd)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                INSERT INTO produce (farmer_id, produce_name, description, price_per_unit, available_quantity, 
                                   unit, farmer_can_deliver, availability_start, availability_end, created_at)
                VALUES (@farmerId, @produceName, @description, @pricePerUnit, @availableQuantity, 
                       @unit, @farmerCanDeliver, @availabilityStart, @availabilityEnd, NOW())";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@farmerId", farmerId);
            command.Parameters.AddWithValue("@produceName", produceName);
            command.Parameters.AddWithValue("@description", description ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@pricePerUnit", pricePerUnit);
            command.Parameters.AddWithValue("@availableQuantity", availableQuantity);
            command.Parameters.AddWithValue("@unit", unit);
            command.Parameters.AddWithValue("@farmerCanDeliver", farmerCanDeliver);
            command.Parameters.AddWithValue("@availabilityStart", availabilityStart);
            command.Parameters.AddWithValue("@availabilityEnd", availabilityEnd);

            await command.ExecuteNonQueryAsync();
            return (int)command.LastInsertedId;
        }

        /// <summary>
        /// Updates produce details
        /// </summary>
        public async Task UpdateProduceAsync(int produceId, string? produceName, string? description,
            decimal? pricePerUnit, int? availableQuantity, string? unit, bool? farmerCanDeliver,
            DateTime? availabilityStart, DateTime? availabilityEnd)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                UPDATE produce 
                SET produce_name = COALESCE(@produceName, produce_name),
                    description = COALESCE(@description, description),
                    price_per_unit = COALESCE(@pricePerUnit, price_per_unit),
                    available_quantity = COALESCE(@availableQuantity, available_quantity),
                    unit = COALESCE(@unit, unit),
                    farmer_can_deliver = COALESCE(@farmerCanDeliver, farmer_can_deliver),
                    availability_start = COALESCE(@availabilityStart, availability_start),
                    availability_end = COALESCE(@availabilityEnd, availability_end)
                WHERE produce_id = @produceId";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@produceId", produceId);
            command.Parameters.AddWithValue("@produceName", produceName ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@description", description ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@pricePerUnit", pricePerUnit ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@availableQuantity", availableQuantity ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@unit", unit ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@farmerCanDeliver", farmerCanDeliver ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@availabilityStart", availabilityStart ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@availabilityEnd", availabilityEnd ?? (object)DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }

        // ==================== ORDER METHODS ====================

        /// <summary>
        /// Creates a new order with order items
        /// </summary>
        public async Task<int> CreateOrderAsync(int schoolId, int farmerId, DateTime orderDate,
            DateTime deliveryDate, List<OrderItemRequest> orderItems)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                // Calculate total cost
                decimal totalCost = 0;
                foreach (var item in orderItems)
                {
                    totalCost += item.Quantity * item.UnitPrice;
                }

                // Create the order
                var orderQuery = @"
                    INSERT INTO orders (school_id, farmer_id, order_date, delivery_date, status, 
                                      total_cost, payment_status, created_at)
                    VALUES (@schoolId, @farmerId, @orderDate, @deliveryDate, 'Pending', 
                           @totalCost, 'Unpaid', NOW())";

                using var orderCommand = new MySqlCommand(orderQuery, connection, transaction);
                orderCommand.Parameters.AddWithValue("@schoolId", schoolId);
                orderCommand.Parameters.AddWithValue("@farmerId", farmerId);
                orderCommand.Parameters.AddWithValue("@orderDate", orderDate);
                orderCommand.Parameters.AddWithValue("@deliveryDate", deliveryDate);
                orderCommand.Parameters.AddWithValue("@totalCost", totalCost);

                await orderCommand.ExecuteNonQueryAsync();
                var orderId = (int)orderCommand.LastInsertedId;

                // Create order items
                foreach (var item in orderItems)
                {
                    var itemQuery = @"
                        INSERT INTO order_items (order_id, produce_id, quantity, unit_price, subtotal)
                        VALUES (@orderId, @produceId, @quantity, @unitPrice, @subtotal)";

                    using var itemCommand = new MySqlCommand(itemQuery, connection, transaction);
                    itemCommand.Parameters.AddWithValue("@orderId", orderId);
                    itemCommand.Parameters.AddWithValue("@produceId", item.ProduceId);
                    itemCommand.Parameters.AddWithValue("@quantity", item.Quantity);
                    itemCommand.Parameters.AddWithValue("@unitPrice", item.UnitPrice);
                    itemCommand.Parameters.AddWithValue("@subtotal", item.Quantity * item.UnitPrice);

                    await itemCommand.ExecuteNonQueryAsync();

                    // Update produce available quantity
                    var updateProduceQuery = @"
                        UPDATE produce 
                        SET available_quantity = available_quantity - @quantity
                        WHERE produce_id = @produceId";

                    using var updateCommand = new MySqlCommand(updateProduceQuery, connection, transaction);
                    updateCommand.Parameters.AddWithValue("@quantity", item.Quantity);
                    updateCommand.Parameters.AddWithValue("@produceId", item.ProduceId);

                    await updateCommand.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();
                return orderId;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Gets order by order ID
        /// </summary>
        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                SELECT order_id, school_id, farmer_id, order_date, delivery_date, status, 
                       total_cost, payment_status, created_at
                FROM orders
                WHERE order_id = @orderId";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@orderId", orderId);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Order
                {
                    OrderId = reader.GetInt32("order_id"),
                    SchoolId = reader.GetInt32("school_id"),
                    FarmerId = reader.GetInt32("farmer_id"),
                    OrderDate = reader.GetDateTime("order_date"),
                    DeliveryDate = reader.GetDateTime("delivery_date"),
                    Status = reader.GetString("status"),
                    TotalCost = reader.GetDecimal("total_cost"),
                    PaymentStatus = reader.GetString("payment_status"),
                    CreatedAt = reader.GetDateTime("created_at")
                };
            }

            return null;
        }

        /// <summary>
        /// Updates order status
        /// </summary>
        public async Task UpdateOrderStatusAsync(int orderId, string status)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "UPDATE orders SET status = @status WHERE order_id = @orderId";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@orderId", orderId);
            command.Parameters.AddWithValue("@status", status);

            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Updates order payment status
        /// </summary>
        public async Task UpdateOrderPaymentStatusAsync(int orderId, string paymentStatus)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "UPDATE orders SET payment_status = @paymentStatus WHERE order_id = @orderId";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@orderId", orderId);
            command.Parameters.AddWithValue("@paymentStatus", paymentStatus);

            await command.ExecuteNonQueryAsync();
        }

        // ==================== PAYMENT METHODS ====================

        /// <summary>
        /// Creates a payment record
        /// </summary>
        public async Task<int> CreatePaymentAsync(Payment payment)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                INSERT INTO payments (order_id, amount, payment_method, transaction_id, payment_date, status)
                VALUES (@orderId, @amount, @paymentMethod, @transactionId, @paymentDate, @status)";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@orderId", payment.OrderId);
            command.Parameters.AddWithValue("@amount", payment.Amount);
            command.Parameters.AddWithValue("@paymentMethod", payment.PaymentMethod);
            command.Parameters.AddWithValue("@transactionId", payment.TransactionId ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@paymentDate", payment.PaymentDate);
            command.Parameters.AddWithValue("@status", payment.Status);

            await command.ExecuteNonQueryAsync();
            return (int)command.LastInsertedId;
        }

        /// <summary>
        /// Gets payment by order ID
        /// </summary>
        public async Task<Payment?> GetPaymentByOrderIdAsync(int orderId)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                SELECT payment_id, order_id, amount, payment_method, transaction_id, payment_date, status
                FROM payments
                WHERE order_id = @orderId
                ORDER BY payment_date DESC
                LIMIT 1";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@orderId", orderId);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Payment
                {
                    PaymentId = reader.GetInt32("payment_id"),
                    OrderId = reader.GetInt32("order_id"),
                    Amount = reader.GetDecimal("amount"),
                    PaymentMethod = reader.GetString("payment_method"),
                    TransactionId = reader.IsDBNull(reader.GetOrdinal("transaction_id")) ? null : reader.GetString("transaction_id"),
                    PaymentDate = reader.GetDateTime("payment_date"),
                    Status = reader.GetString("status")
                };
            }

            return null;
        }

        // ==================== DELIVERY METHODS ====================

        /// <summary>
        /// Creates or assigns a delivery for an order
        /// </summary>
        public async Task<int> CreateDeliveryAsync(int orderId, string? truckCompany, string? truckContact,
            decimal deliveryFeeTotal, DateTime estimatedArrival)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            // Calculate 50/50 split
            var schoolShare = deliveryFeeTotal / 2;
            var farmerShare = deliveryFeeTotal / 2;

            var query = @"
                INSERT INTO deliveries (order_id, truck_company, truck_contact, delivery_fee_total, 
                                      school_share, farmer_share, delivery_status, estimated_arrival)
                VALUES (@orderId, @truckCompany, @truckContact, @deliveryFeeTotal, 
                       @schoolShare, @farmerShare, 'Scheduled', @estimatedArrival)";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@orderId", orderId);
            command.Parameters.AddWithValue("@truckCompany", truckCompany ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@truckContact", truckContact ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@deliveryFeeTotal", deliveryFeeTotal);
            command.Parameters.AddWithValue("@schoolShare", schoolShare);
            command.Parameters.AddWithValue("@farmerShare", farmerShare);
            command.Parameters.AddWithValue("@estimatedArrival", estimatedArrival);

            await command.ExecuteNonQueryAsync();
            return (int)command.LastInsertedId;
        }

        /// <summary>
        /// Gets delivery by order ID
        /// </summary>
        public async Task<Delivery?> GetDeliveryByOrderIdAsync(int orderId)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                SELECT delivery_id, order_id, truck_company, truck_contact, delivery_fee_total, 
                       school_share, farmer_share, delivery_status, estimated_arrival
                FROM deliveries
                WHERE order_id = @orderId";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@orderId", orderId);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Delivery
                {
                    DeliveryId = reader.GetInt32("delivery_id"),
                    OrderId = reader.GetInt32("order_id"),
                    TruckCompany = reader.IsDBNull(reader.GetOrdinal("truck_company")) ? null : reader.GetString("truck_company"),
                    TruckContact = reader.IsDBNull(reader.GetOrdinal("truck_contact")) ? null : reader.GetString("truck_contact"),
                    DeliveryFeeTotal = reader.GetDecimal("delivery_fee_total"),
                    SchoolShare = reader.GetDecimal("school_share"),
                    FarmerShare = reader.GetDecimal("farmer_share"),
                    DeliveryStatus = reader.GetString("delivery_status"),
                    EstimatedArrival = reader.GetDateTime("estimated_arrival")
                };
            }

            return null;
        }

        /// <summary>
        /// Updates delivery status
        /// </summary>
        public async Task UpdateDeliveryStatusAsync(int deliveryId, string deliveryStatus)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "UPDATE deliveries SET delivery_status = @deliveryStatus WHERE delivery_id = @deliveryId";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@deliveryId", deliveryId);
            command.Parameters.AddWithValue("@deliveryStatus", deliveryStatus);

            await command.ExecuteNonQueryAsync();
        }
    }
}

