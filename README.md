# 📚 Book Reviews – ASP.NET Core MVC + REST API

This is a full-stack web application for managing and reviewing books. Users can register, log in, post reviews, and vote on others’ reviews. The app also exposes a RESTful API.

## 🛠 Technologies Used

- **.NET 9 / ASP.NET Core MVC**
- **Entity Framework Core** (Code-First + Migrations)
- **ASP.NET Core Identity**
- **SQLite** (for local development)
- **Razor Pages / Views**
- **RESTful API with [ApiController]**
- Swagger UI for API documentation

---

## 🔐 Features

### ✅ Authentication & Authorization
- User Registration & Login via **ASP.NET Core Identity**
- Only **authenticated users** can:
  - Submit reviews
  - Upvote/downvote reviews

### 📚 Book Management (MVC)
- Add/edit/delete books (admin/user)
- Filter book list by:
  - Genre
  - Published Year
  - Author (search with partial match)

### ✍️ Reviews
- Users can submit reviews with:
  - Text content
  - Rating (1–5)
- Reviews display author, content, date, and rating

### 👍 Review Voting
- Users can upvote/downvote a review
- Only **one vote per review per user**
- Same vote triggers a friendly validation message

---

## 🔌 REST API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET    | `/api/books` | List all books (with filters) |
| GET    | `/api/books/{id}` | Book details |
| POST   | `/api/books` | Create a new book |
| GET    | `/api/books/{id}/reviews` | Reviews for a book |
| POST   | `/api/reviews` | Add a review |
| POST   | `/api/reviews/{id}/vote` | Vote on a review (up/down) |

---

