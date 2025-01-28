Feature: Pagamento

Payment operations related feature

@pagamentos
Scenario: QR Code for Payment Request Also Saves Payment in the Database
	Given an order of id '92ecd243-41d3-4400-9a5c-4f95531f0c8c'
	When the application is called to generate a QR Code for payment
	Then the database is called to save the payment