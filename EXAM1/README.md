# Ticket Booking API

**Sistem REST API untuk Manajemen Pemesanan Tiket**

---

## ?? Daftar Isi

- [Deskripsi Proyek](#deskripsi-proyek)
- [Tech Stack](#tech-stack)
- [Fitur Utama](#fitur-utama)
- [Arsitektur](#arsitektur)
- [Persyaratan Sistem](#persyaratan-sistem)
- [Setup & Instalasi](#setup--instalasi)
- [Konfigurasi Database](#konfigurasi-database)
- [Konfigurasi Logging](#konfigurasi-logging)
- [API Documentation](#api-documentation)
- [Error Handling](#error-handling)
- [Best Practices](#best-practices)
- [Testing](#testing)
- [Deployment](#deployment)
- [Troubleshooting](#troubleshooting)

---

## ?? Deskripsi Proyek

**Ticket Booking API** adalah REST API yang dirancang untuk mengelola sistem pemesanan tiket acara. API ini menyediakan fitur lengkap untuk:

- Melihat daftar tiket yang tersedia
- Melakukan pemesanan tiket dengan validasi quota
- Mengelola pemesanan (edit, revoke)
- Tracking status pemesanan

**Status Project:** ? Production Ready

---

## ??? Tech Stack

| Component | Technology | Version |
|-----------|-----------|---------|
| **Framework** | ASP.NET Core | 10.0 |
| **Language** | C# | 14.0 |
| **ORM** | Entity Framework Core | Latest |
| **Database** | SQL Server | 2019+ / Express |
| **Pattern** | MediatR (CQRS) | Latest |
| **Validation** | FluentValidation | Latest |
| **Logging** | Serilog | Latest |
| **Exception Handling** | RFC 7807 ProblemDetails | Standard |
| **API Documentation** | Swagger/OpenAPI | 3.0 |

---

## ? Fitur Utama

### 1. **Get Available Tickets**
- Retrieve list tiket yang tersedia
- Filter berdasarkan: Code, Name, Price, EventDate
- Sorting: Price, EventDate, Code
- Response include kategori tiket

### 2. **Book Ticket**
- Pemesanan tiket dengan validasi lengkap
- Validasi:
  - ? Ticket code harus terdaftar
  - ? Quota ticket harus tersedia
  - ? Quantity tidak boleh melebihi quota
  - ? Tanggal event harus > tanggal booking
- Response include:
  - Detail tiket yang dibooking
  - Total per kategori
  - Grand total

### 3. **Get Booked Ticket Detail**
- Retrieve detail pemesanan dengan items lengkap
- Include kategori dan harga per item

### 4. **Edit Booked Ticket**
- Update quantity tiket yang sudah dibooking
- Otomatis update quota ticket
- Support increase dan decrease quantity

### 5. **Revoke Ticket**
- Cancel pemesanan tiket (sebagian atau seluruh)
- Otomatis update quota
- Auto-delete booking jika tidak ada items

---

## ??? Arsitektur
### **MARVEL Pattern dengan MediatR**

## Setup Database
- Menggunakan EF Core Migrations
