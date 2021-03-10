# LibraryAPI
REST API aplikacji bibliotecznej, utworzonej w ASP.net core 5.

## Metody przekazywania danych

| Wywołania GET  | Opis | 
| ------------- | ------------ | 
| ../api/books  | Książki - wszystkie  | 
| ../api/books/{id} | Książki - pojedyncza wartość  | 
|   | Książki - ilość dostępnych książek (dla danego ID)  | 
|   | Dla Usera - pobranie listy wypożyczonych książek (z datą zwrotu)  | 
|   | User (pracownik) - pobranie listy wypożyczonych książek dla ID usera  | 
|   | User (pracownik) - pobranie listy wypożyczonych książek po terminie  | 
| ../api/users/{id}  | User (pracownik) - pobranie danych po ID Usera | 
| ../api/users | User (pracownik) - pobranie wszystkich userów |

| Wywołania PUT  | Opis | 
| ------------- | ------------ | 
| ../api/users/{id}  | User (pracownik) - zmiana Imienia, nazwiska, emaila Usera  | 
| ../api/books/{id} | User (pracownik) - edycja książki  | 
|   | User (pracownik) - zmiana hasła  | 
|   | User (pracownik) - zmiana daty oddania książki na podstawie ID usera  | 
|   | User - zmiana hasła  | 
|   | User (pracownik) - potwierdza wypoż./zwrot książki  | 
|   | User - rezerwacja  | 

| Wywołania POST  | Opis | 
| ------------- | ------------ | 
| ../api/books  | User (pracownik) - dodanie nowej książki  | 
| ../api/users | User (pracownik) - dodanie Usera  | 
|   | User (admin) - dodanie nowego Usera (pracownik)  | 

| Wywołania DELETE  | Opis | 
| ------------- | ------------ | 
| ../api/books/{id}  | User (pracownik) - usuwanie książki  | 
| ../api/users{id} | User (pracownik) - usuwanie Usera  | 
|   | User (admin) - usuwanie User (pracownik)  | 