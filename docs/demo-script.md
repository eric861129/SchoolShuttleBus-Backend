# Demo Script

## 1. Open Swagger

- Open `/swagger/index.html`
- Explain this is a backend-first shuttle bus management API
- Mention CI/CD and Azure deployment are already in place

## 2. Authentication

- Log in with `E0001 / P@ssw0rd!`
- Paste the returned access token into Swagger `Authorize`
- Call `GET /api/auth/me`

## 3. Parent Flow

- Log in with `0910-200-001 / P@ssw0rd!`
- Call registration endpoints
- Show weekly registration and summary data

## 4. Teacher Flow

- Log in with `T0001 / P@ssw0rd!`
- Call `GET /api/routes`
- Create or review attendance sessions
- Update attendance records

## 5. Student Flow

- Log in with `S20001 / P@ssw0rd!`
- Show student-facing registration queries

## 6. Admin Flow

- Use dispatch override endpoints
- Show broadcast / reminder related data
- Export reports

## 7. Technical Wrap-Up

- Explain JWT-based auth
- Explain EF Core migrations and seeded/demo SQL data
- Explain Azure App Service + Azure SQL deployment
