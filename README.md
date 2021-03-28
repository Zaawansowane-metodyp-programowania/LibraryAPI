# LibraryAPI
REST API aplikacji bibliotecznej, utworzonej w ASP.net core 5.

## Metody przekazywania danych

| Wywołania GET  | Opis | 
| ------------- | ------------ | 
| ../api/books  | Książki - wszystkie  | 
| ../api/books/{id} | Książki - pojedyncza wartość  | 
| ../api/books/user/{userid}  | Książki - ilość dostępnych książek (dla danego ID)  | 
| ../api/books/user/{userid} | User (pracownik, admin) - pobranie listy wypożyczonych książek dla ID usera  | 
| ../api/users/{id}  | User (pracownik,admin) - pobranie danych po ID Usera | 
| ../api/users | User (pracownik,admin) - pobranie wszystkich userów |

| Wywołania PUT  | Opis | 
| ------------- | ------------ | 
| ../api/users/{id}  | User  - zmiana Imienia, nazwiska, emaila (każdy dla siebie, admin może dla wszystkich) | 
| ../api/books/{id} | User (pracownik,admin) - edycja książki  | 
| ../api/books/{id} | User (pracownik,admin) - potwierdza wypoż./zwrot książki  | 
| ../api/books/reservation/{id} | User - rezerwacja  | 

| Wywołania PATCH  | Opis | 
| ------------- | ------------ | 
|../api/users/changePassword/{id}  | - zmiana hasła (każdy dla siebie, admin może dla wszystkich)  |   | 

| Wywołania POST  | Opis | 
| ------------- | ------------ | 
| ../api/books  | User (pracownik,admin) - dodanie nowej książki  | 
| ../api/users | User (admin) - dodanie nowego użytkownika, pracownika lub admina | 
| ../api/account/register | rejestracja Usera  |
| ../api​/account​/login    |  logowanie użytkownika |

| Wywołania DELETE  | Opis | 
| ------------- | ------------ | 
| ../api/books/{id}  | User (pracownik,admin) - usuwanie książki  | 
| ../api/users{id} | Usunać może każdy siebie, admin może wszystkich | 
