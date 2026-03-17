# API 概覽

## Auth

- `POST /api/auth/login`
- `POST /api/auth/refresh`
- `POST /api/auth/logout`
- `GET /api/auth/me`

## Registrations

- `GET /api/registrations/weeks/{weekStart}?studentId={studentId}`
- `PUT /api/registrations/weeks/{weekStart}`
- `POST /api/registrations/weeks/{weekStart}/copy-last-week?studentId={studentId}`
- `GET /api/registrations/students/{studentId}/summary`

## Routes

- `GET /api/routes`
- `POST /api/routes`
- `PATCH /api/routes/{routeId}`
- `POST /api/routes/{routeId}/stops`
- `POST /api/routes/assignments/{routeId}`

## Attendance

- `GET /api/attendance/sessions`
- `POST /api/attendance/sessions`
- `PATCH /api/attendance/records/{recordId}`
- `POST /api/attendance/sessions/{sessionId}/complete`

## Notifications

- `POST /api/notifications/reminders/run`
- `GET /api/notifications/history`

## Admin

- `POST /api/admin/dispatches`
- `POST /api/admin/broadcasts`
- `POST /api/admin/reports`
- `GET /api/admin/reports/{reportId}`

## Bearer Token

Swagger 已內建 `Bearer` security scheme。登入後把 `accessToken` 貼進 Swagger Authorize 對話框即可。
