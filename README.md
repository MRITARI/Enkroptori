# Enkröptori 🔐

A console-based AES file encryption and decryption tool written in C#.  
Built for simplicity, safety, and a little bit of style.

---

## 💡 What is Enkröptori?

Enkröptori is a lightweight command-line tool that allows you to encrypt and decrypt files using AES-256 encryption. It includes a simple interface, clipboard support for generated credentials, and maintains a local encryption/decryption history.

---

## ⚙️ Features

- 🔒 AES-256 encryption and decryption
- 📄 History tracking (`encrypthistory.txt`)
- 📋 Clipboard copy of keys/IVs for convenience
- 💀 Warns if you lose your credentials — you won't recover your files!
- 🧼 History clearing option
- 🧙 Simple terminal interface with helpful prompts

---

## 🛠️ Usage

Run the application via console or with arguments:

```bash
Enkroptori.exe [options]
