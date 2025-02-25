# 🎓 UniPath Capsule System - MVC

## 🌟 Project Overview
Welcome to **UniPath Capsule System**, an innovative **e-learning platform** designed to enhance student engagement through interactive educational capsules. Built using **Microsoft .NET Core MVC**, this project brings a structured and immersive learning experience to users.

This project implements the **Complete Capsule** use case of the full **UniPath Project**, allowing students to access educational capsules, engage with interactive content, and track their progress.

---

## 🎯 Project Goal
The UniPath Capsule System is designed to deliver **video-based learning modules** with integrated quizzes to assess student comprehension. The system tracks user progress, enabling a **seamless and effective learning journey**.

---

## 💡 Key Features
✔ **Video Learning Capsules** – Engaging instructional videos for each topic.  
✔ **Interactive True/False Quizzes** – Reinforce learning through assessments.    

---

## 🛠️ Technical Implementation
This project follows the **MVC (Model-View-Controller)** architecture:

### **MVC Structure**
- **Model**: Handles business logic and data interactions.
- **View**: Presents dynamic content to users (Razor views).
- **Controller**: Manages requests and responses.

### **Current Implementation**
✅ **Video-based capsules** with learning content.  
✅ **True/False quizzes** for knowledge assessment.  

---

## 📚 Complete Capsule Use Case Workflow
The **Complete Capsule** use case allows a **student** to:
- View the content of a **Capsule**.
- Watch an associated **educational video**.
- Answer **True/False questions** related to the capsule.
- Successfully complete the capsule and move on to the next one.

---

### **👥 Actors**
- **Student**: A registered user who interacts with Capsules.

---

### **🛠 Workflow Steps**
#### **1️⃣ Log In**
- The student logs into the platform using **credentials**.
- **Pre-requisite**: The student must be registered.

🗂 **Required Data**
| Field     | Description                      |
|-----------|----------------------------------|
| Username  | The user's unique identifier.   |
| Email     | The user's registered email.    |
| Password  | The user's secure password.     |

---

#### **2️⃣ Navigate to Capsule Page**
- The student can access a **Capsule** in two ways:
  - From the **Login Page**.
  - From the **Class Page** → Navigating to a specific **Capsule Page**.

---

#### **3️⃣ Read Content & Watch Video**
- The student views the **Capsule's video** and reads the **provided notes**.

🗂 **Capsule Data**
| Field               | Description                          |
|---------------------|--------------------------------------|
| Capsule Video URL  | Link to the educational video.       |
| Capsule Content    | Text-based learning material.        |

---

#### **4️⃣ Answer True/False Questions**
- The student answers **True/False** questions related to the capsule.

🗂 **Question Data**
| Field        | Description                                |
|-------------|--------------------------------------------|
| Question ID | Unique identifier of the question.        |
| Answer      | True/False response from the student.     |
| CorrectAnswer | The correct answer for validation.      |

---

#### **5️⃣ Completion and Redirection**
- If the student answers **all questions correctly**, their completion is recorded.
- They are **redirected to the next Capsule** or, if none exists, back to the **Class Page**.
- **If they answer incorrectly**, they receive feedback and can **retry**.

🗂 **Completion Tracking**
| Field          | Description                              |
|---------------|------------------------------------------|
| Capsule ID    | The unique identifier of the capsule.   |
| Student ID    | The unique identifier of the student.   |
| Completion    | Boolean indicating completion status.   |

---

## 🔄 Alternative Flow: Wrong Answers
If the student **answers a question incorrectly**:
- The system **provides immediate feedback**.
- The student can **retry** the questions.
- They can only **progress** after answering **all questions correctly**.

---

## 🚀 Getting Started

### **🔧 Prerequisites**
- .NET 9 SDK
- Microsoft SQL Server
- Visual Studio 2022 (or VS Code)
- Git for version control

### **💻 Installation Steps**
1. **Clone the repository**
   ```sh
   git clone https://github.com/your-repo/UniPath-Capsule-System.git
   cd UniPath-Capsule-System
