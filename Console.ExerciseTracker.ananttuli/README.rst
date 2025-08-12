Exercise Tracker
================

Log exercise sessions and track progress.

Run locally
-----------

1. Clone this repository
2. ``cd <Root>/App``
3. ``dotnet ef database update``
4. ``dotnet run``

Configuration (optional)
------------------------

Default configuration should work as-is, but can be customised if
desired:

-  Change SQLite database path in ``App/App.config`` (default should
   work)

Tech stack
----------

-  C#
-  EntityFramework Core ORM
-  SQLite