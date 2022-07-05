# Readme
## Description of the solution
1. Domain driven design as the key principle.
2. Solution was made strictly according to technical requirement, so API resources left with their original contracts.
3. Added feature - **configurable games**. We can add any new *rock-paper-scissors*-like game via configuration file.  
4. Added feature - **scoreboard**. We can request it for each available game type.
5. Added feature - **game strategy** for computer player. 


## TODO
1. Add logging and monitoring.
2. Add DI feature of auto resolve games by config, so we wont have to manual register each new game in DI.
3. Add async calls.
4. Remove some hardcode.
5. Add component and integrational tests.
