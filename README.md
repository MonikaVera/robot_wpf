# Robot Construction Game

The Robot Construction Game is designed to foster teamwork, strategic planning, and problem-solving skills. Players must work together to manipulate robots on a shared field, construct predefined structures from blocks, and remove these structures from the field through designated exits. Each robot, controlled by a player, can move, rotate, and interact with blocks and other robots. The game emphasizes coordination and communication among team members to efficiently complete tasks and overcome challenges.

## Game Features

### Core Gameplay

1. **Team-Based Robot Control**
   - Players control robots that recognize team members by color.
   - Robots identify their position in the team by number.

2. **Grid-Based Field**
   - Robots operate on an 12 * 10 grid of cells.
   - Cells can contain colored blocks, obstacles, robots, exits, or be empty.

3. **Discrete Actions**
   - Robots act in discrete steps with 30 seconds to initiate an action.
   - If no action is initiated, the robot does nothing for that round.

4. **Limited Visibility**
   - Robots perceive the contents of cells within a 3 Manhattan distance.

5. **Block Interaction**
   - Robots connect to blocks to move them.
   - Robots must disconnect from blocks to move independently.
   - Connected blocks can move together if robots agree on the direction.

6. **Obstacles and Exits**
   - Robots can't move through obstacles but can clear them with repeated actions.
   - Robots can't leave the field but can push blocks out through exits to earn points.

### Advanced Gameplay

1. **Task Board**
   - Tasks appear on a task board visible to all robots.
   - Tasks have a name, deadline, reward points, and a specification for block arrangements.
   - Completing tasks before the deadline earns reward points.

2. **Environmental Awareness**
   - Robots update their perception of the field after each action.

3. **Communication**
   - Robots and players can send messages to each other.
   - Messages can be sent to specific team members or broadcasted.

4. **Random Initialization**
   - Robots, exits, blocks, and obstacles are placed randomly at the start.

5. **Game Duration**
   - The game lasts for 21 steps.

### Player Interaction

1. **Graphical Interface**
   - Players control robots through an intuitive graphical interface.
   - Players see only what their robot perceives, stored from previous observations.

2. **Spectator View**
   - A "game master" and audience can view the entire field.
   - Option to log the game for later review.

### Additional Features

1. **Simple Map Display**
   - Robots store and display discovered areas on a map.
   - Updates the map when changes are detected.

2. **Shared Map Display**
   - Robots combine their maps with information from identified teammates.
   - Display the boundaries of the field.

3. **Communication System**
   - Develop a system for quick communication between players.
   - Options for standard messages, shared boards, or task annotations.

4. **Additional Game Rules**
   - Robots can delete each other

### Robot Actions

1. **Wait**
   - Does nothing for the round. Always successful.
   - Contolled with V

2. **Move**
   - Moves to an adjacent cell (N, E, S, W). Moves attachments with it. Fails if the target cell is not empty.
   - Controlled with W (up), A (left), S (down), D (right).

3. **Rotate**
   - Rotates 90 degrees (clockwise or counterclockwise). Moves attachments with it. Fails if target cells are not empty.
   - Controlled with E (rotate east), Q (rotate west)

4. **Connect**
   - Connects to a block in the direction which the robot is facing. Fails if no block is present.
   - Controlled with N

5. **Disconnect**
   - Disconnects from a blocks. Fails if no block is present.
   - Controlled with X

6. **Link**
   - Links two blocks in specified relative positions. Requires another robot to link the same blocks simultaneously.
   - Controlled with M

7. **Unlink**
   - Unlinks two blocks in specified relative positions. Fails if no block is present or parameters are invalid.
   -  Controlled with B

8. **Clean**
   - Cleans a cell in a specified direction which the robot is facing.. After \(c\) cleanings, the cell becomes empty.
   - Controlled with C

### Robot Perception

1. **Operation Success**
   - Feedback on whether the last action was successful.

2. **Visible Cells**
   - Contents of cells within the robot's visibility range, including connections.

3. **Team Points**
   - Current points accumulated by the robot's team.

4. **Task Board**
   - List of tasks available on the task board.

## Technical Architecture
The Robot Construction Game is developed using Windows Presentation Foundation (WPF), leveraging the Model-View-ViewModel (MVVM) pattern and a persistence architecture to ensure a robust and maintainable codebase.

### Key Technical Features:
1. WPF Framework: Utilizes the WPF framework for building a rich, interactive, and user-friendly graphical interface.
2. MVVM Pattern: Implements the Model-View-ViewModel pattern to separate the application's logic from the user interface, promoting a clean and modular design.
   - Model: Represents the application's data and business logic.
   - View: Defines the user interface, including layout and controls, to display the application's data.
   - ViewModel: Acts as an intermediary between the Model and View, handling user interactions and updating the View.
3. Persistence Architecture: Ensures that game state, player progress, and other important data are saved and can be retrieved reliably, enhancing the game's durability and user experience.
