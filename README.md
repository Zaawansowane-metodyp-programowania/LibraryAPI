# LibraryAPI
REST API aplikacji bibliotecznej, utworzonej w ASP.net core 5.

## Metody przekazywania danych

| Wywołania GET  | Opis | 
| ------------- | ------------ | 
| ../api/books  | Książki - wszystkie  | 
| ../api/books/{id} | Książki - pojedyncza wartość  | | 
| ../api/books/user/{userid} | User (pracownik i admin dla wszystkich, zwykły użytkownik dla siebie) - pobranie listy wypożyczonych książek dla ID usera  | 
| ../api/users/{id}  | User - pobranie danych po ID Usera (zwykły użytkownik tylko siebie, admin i pracownik wszystkich) | 
| ../api/users | User (pracownik,admin) - pobranie wszystkich userów |

| Wywołania PUT  | Opis | 
| ------------- | ------------ | 
| ../api/users/{id}  | User  - zmiana Imienia, nazwiska, emaila (każdy dla siebie, admin może dla wszystkich) | 
| ../api/books/{id} | User (pracownik,admin) - edycja książki  | 



| Wywołania PATCH  | Opis | 
| ------------- | ------------ | 
|../api/users/changePassword/{id}  | zmiana hasła (każdy dla siebie, admin może dla wszystkich)  |   | 
|../api/books/reservation/{id} | User - rezerwacja  | 
|../api/books/borrow/{id} | Pracownik - wypożyczenie ksiązki dla użytkownika |
|../api/books/return/{id} | Pracownik - zwrot ksiązki od użytkownika  |

| Wywołania POST  | Opis | 
| ------------- | ------------ | 
| ../api/books  | User (pracownik,admin) - dodanie nowej książki  | 
| ../api/users | User (admin) - dodanie nowego użytkownika, pracownika lub admina | 
| ../api/account/register | rejestracja użytkownika |
| ../api​/account​/login    |  logowanie użytkownika |

| Wywołania DELETE  | Opis | 
| ------------- | ------------ | 
| ../api/books/{id}  | User (pracownik,admin) - usuwanie książki  | 
| ../api/users{id} | usuwanie użytkownika(Usunać może każdy siebie, admin może wszystkich) | 
