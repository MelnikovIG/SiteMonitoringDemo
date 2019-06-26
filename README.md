## Site monitoring demo app

Simple app for tracking sites availability. It is able to add, update, remove sites, configure status update period for each site. Site is considered alive, if it can respond with success status code within short amount of time (1 sec by default). App uses in-memory storage, so data is not persisted.

## Tested OS:
- Windows

## Tested browsers:
- Google Chrome

## How to build and run app
Prerequisites: .NET Core 2.2 sdk required to build app (included in latest versions of VisualStudio)

1) Run runWebApp.cmd file. It will build and run webapp. By default app listening 5000 port.
2) Open browser at url http://localhost:5000/

Note: there is some predefined urls to test app
- http://localhost:5000/alwaysOk always returns NoContent(204) stattus
- http://localhost:5000/alwaysFail always returns InternalServerError(500) stattus
- http://localhost:5000/alwaysLong always returns NoContent(204) stattus, but with long response (considered as failure)
- http://localhost:5000/random selects randomly between NoContent(204) and InternalServerError(500) status

## Tests
- Unit tests located at .\test\unit
- Integration tests located at .\test\integration
