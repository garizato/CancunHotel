# Cancun Hotel

## CHALLENGE
### Post-Covid scenario:
People are now free to travel everywhere but because of the pandemic, a lot of hotels went bankrupt. Some former famous travel places are left with only one hotel.
You’ve been given the responsibility to develop a booking API for the very last hotel in Cancun.

`The requirements are:`
  -	API will be maintained by the hotel’s IT department.
  -	As it’s the very last hotel, the quality of service must be 99.99 to 100% => no downtime
  -	For the purpose of the test, we assume the hotel has only one room available
  -	To give a chance to everyone to book the room, the stay can’t be longer than 3 days and can’t be reserved more than 30 days in advance.
  -	All reservations start at least the next day of booking,
  -	To simplify the use case, a “DAY’ in the hotel room starts from 00:00 to 23:59:59.
  -	Every end-user can check the room availability, place a reservation, cancel it or modify it.
  -	To simplify the API is insecure.

## SOLUTION
For the solution of the challenge I have built the followings methods:

### Booking

  `\Insert`
  
  This method allows you to record guest booking by recording the dates of the reservation and the applicant's information.
  
  **Request Parameters**
  
      - startDateRequired: Format `2022-07-22T00:00:00.000`
      - finalDateRequired: Format `2022-07-24T00:00:00.000`
      
  **Request body**
  
    {
      "guestName": "string",
      "documentNumber": "string",
      "phoneNumber": "string"
    }

  `\Cancel`
  
  This method allows you to cancel a booking through the booking number.
  
  **Request Parameter**
  
      - bookingId 
  
  `\Update`
  
  This method allows you to modify an existing booking
    
  **Request Parameters**
    
      - bookingId
      - startDateRequired: Format `2022-07-22T00:00:00.000`
      - finalDateRequired: Format `2022-07-24T00:00:00.000`
  
### Room

  `\CheckAvailableRoom`
  
  This method allows you to check the dates on wich the room is reserved
  
  **Requested Parameters**
  
      - roomNumber
