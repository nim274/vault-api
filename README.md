# vault-api
	This api is a vault to store auth keys which are used to connect with external systems


## Notes
- __database:__ Vault-api uses `in-memory` database, which gets destroyed when application stops
- Swagger url will provide endpoint details
- Sample vendor name: Cobalt

## To load app on docker use below commands

- Build image
												
	###### docker build -t vault-api .

- Run docker image

	###### docker run -d -p 8080:8080 vault-api
