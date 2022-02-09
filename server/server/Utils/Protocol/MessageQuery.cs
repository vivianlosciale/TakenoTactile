namespace server.Utils.Protocol;

public enum MessageQuery
{
    
    /***************************************
                   INFORMATIVE
     ***************************************/

    /**
     * FROM/TO: everyone -> everyone
     * NO ARGS
     *     To do nothing
     */
    None,
    
    /**
     * FROM/TO: everyone -> everyone
     * ARGS:    a string message
     *     To ping a device
     */
    Ping,
    
    /**
     * FROM/TO: everyone -> everyone
     * ARGS:    a string error
     *     To warn of an error
     */
    Error,
    
    
    
    /***************************************
                  CONNECTIONS
     ***************************************/
    
    /**
     * FROM/TO: table -> server
     * ARGS:     a unique identifier
     *     To inform the server the table requires a connection
     */
    TableConnection,
    
    /**
     * FROM/TO: mobile -> server
     * ARGS:    a unique identifier
     *     To inform the server the mobile requires a connection
     */
    PlayerConnection,
    
    /**
     * FROM/TO: server -> everyone
     * ARGS:    the device unique identifier, the private socket address
     *     To inform a device its connection was accepted and
     *     to give it the new private socket to communicate
     */
    AcceptConnection,
    
    /**
     * FROM/TO: server -> table
     * ARGS:    the player number
     *     To inform the table that a player joined the game
     */
    APlayerJoined,
    
    /**
     * FROM/TO: server -> everyone
     * NO ARGS
     *     To inform that the game is full and no more players can join
     */
    GameIsFull,
    
    /**
     * FROM/TO: table -> server
     * NO ARGS
     *     To ask the server to start the game
     *
     * FROM/TO: server -> everyone
     * NO ARGS
     *     To inform the game started
     */
    StartGame,
    
    
    
    /***************************************
                  START TURN
     ***************************************/
    
    /**
     * FROM/TO: server -> table
     * ARGS:    the player number
     *     To send the table the current player number
     */
    CurrentPlayerNumber,
    
    
    
    /***************************************
                 ROLL THE DICE
     ***************************************/

    /**
     * FROM/TO: server -> mobile
     * NO ARGS
     *     Inform the mobile the server awaits a dice face
     */
    WaitingDiceResult,
    
    /**
     * FROM/TO: mobile -> server
     * FROM/TO: server -> table
     * ARGS:    the rolled face name
     *     Send the dice rolled face
     */
    RollDice,
    
    
    
    /***************************************
               CHOICE DICE POWER
     ***************************************/
    
    // TODO
    
    
    
    /***************************************
               CLOUD DICE POWER
     ***************************************/
    
    // TODO
    
    
    
    /***************************************
                RAIN DICE POWER
     ***************************************/
    
    /**
     * FROM/TO: server -> everyone
     * NO ARGS
     *     Inform that the server awaits the table
     *     to send a placed tile position
     */
    WaitingChoseRain,
    
    /**
     * FROM/TO: server -> mobile
     * ARGS:    a boolean success status
     *     Inform the mobile the action failed or succeeded
     *
     * FROM/TO: server -> table
     * ARGS:    the position dto where the rain should fall
     *     Inform that the server awaits the table
     *     to send a placed tile position
     */
    RainPower,
    
    
    
    /***************************************
              THUNDER DICE POWER
     ***************************************/
    
    // TODO
    
    
    
    /***************************************
                 CHOSE ACTIONS
     ***************************************/

    /**
     * FROM/TO: server -> everyone
     * NO ARGS
     *     Inform that the server awaits the table
     *     to send information about the chosen actions
     */
    WaitingChoseAction,
    
    /**
     * FROM/TO: table -> server
     * ARGS:    the chosen action name
     *     Inform the server of a chosen action
     */
    ChoseAction,
    
    /**
     * FROM/TO: table -> server
     * ARGS:    the removed action name
     *     Inform the server of a removed action
     */
    RemoveAction,
    
    /**
     * FROM/TO: server -> mobile
     * ARGS:    a boolean indicating if a choice is made
     *     Inform the mobile if it should ask for a validation
     *
     * FROM/TO: mobile -> server
     * FROM/TO: server -> table
     * NO ARGS
     *     Inform the choice have been validated
     */
    ValidateChoice,
    
    
    
    /***************************************
               MOVE FARMER ACTION
     ***************************************/
    
    // TODO
    
    
    
    /***************************************
               MOVE PANDA ACTION
     ***************************************/
    
    // TODO
    
    
    
    /***************************************
                PICK CARD ACTION
     ***************************************/
    
    /**
     * FROM/TO: server -> everyone
     * NO ARGS
     *     Inform that the server awaits the table to pick a card
     */
    WaitingPickCard,
    
    /**
     * FROM/TO: table -> server
     * NO ARGS
     *     Inform the server a pick card request is made
     */
    PickCard,
    
    /**
     * FROM/TO: server -> mobile
     * ARGS:    the received card name
     *     To give the picked card to the mobile
     */
    ReceivedCard,
    
    
    
    /***************************************
               PLACE TILE ACTION
     ***************************************/
    
    /**
     * FROM/TO: server -> everyone
     * NO ARGS
     *     Inform that the server awaits the table to pick tiles
     */
    WaitingPickTiles,
    
    /**
     * FROM/TO: table -> server
     * NO ARGS
     *     Inform the server a pick tiles request is made
     */
    PickTiles,
    
    /**
     * FROM/TO: server -> mobile
     * ARGS:    a list of the picked tiles names
     *     Inform that the server awaits the mobile to chose a card
     */
    WaitingChoseTile,
    
    /**
     * FROM/TO: mobile -> server
     * ARGS:    the name of the chosen tile
     *     To give the chosen tile to the server
     */
    ChosenTile,
    
    /**
     * FROM/TO: server -> table
     * ARGS:    the name of the tile to be placed
     *     Inform that the server awaits the table to return the placed tile position
     */
    WaitingPlaceTile,
    
    /**
     * FROM/TO: server -> mobile
     * NO ARGS
     *     Inform that the chosen tile have been placed
     */
    TilePlaced,
    
    
    
    /***************************************
             CHOSE A BOARD POSITION
     ***************************************/
    
    /**
     * FROM/TO: table -> server
     * ARGS:    the chosen position dto
     *     To send a chosen position to the server (multiple uses)
     */
    ChosenPosition,
    
    
    
    /***************************************
               FINISH THE TURN
     ***************************************/
    
    /**
     * FROM/TO: mobile -> server
     * ARGS:    the objective name
     *     To ask the server to try validating an objective
     *
     * FROM/TO: server -> mobile
     * ARGS:    the objective name
     *     To inform the mobile the objective was validated
     */
    ValidateObjective,
    
    /**
     * FROM/TO: server -> mobile
     * ARGS:    the objective name
     *     To inform the mobile the objective can't be validated
     */
    InvalidObjective,
    
    
    
    /***************************************
               FINISH THE TURN
     ***************************************/
    
    /**
     * FROM/TO: mobile -> server
     * NO ARGS
     *     To inform that the mobile awaits the server to send the food quantity
     */
    WaitingFoodStorage,
    
    /**
     * FROM/TO: server -> mobile
     * ARGS:    the food quantity as a bamboo dto
     *     To send the food quantity to the mobile
     */
    FoodStorage,
    
    
    
    /***************************************
               FINISH THE TURN
     ***************************************/
    
    /**
     * FROM/TO: server -> mobile
     * NO ARGS
     *     Inform that the server awaits the mobile to end the turn
     */
    WaitingEndTurn,
    
    /**
     * FROM/TO: mobile -> server
     * NO ARGS
     *     Inform the server of the end of the turn
     */
    FinishTurn,
}



public static class QueryMethods
{
    public static string ToString( MessageQuery query)
    {
        
        string? res = Enum.GetName(typeof(MessageQuery), query);
        if (res == null) return "None";
        return res;
    }

    public static MessageQuery ToQuery(string query)
    {
        try
        {
            return (MessageQuery)Enum.Parse(typeof(MessageQuery), query);
        }
        catch (Exception)
        {
            return MessageQuery.None;
        }
    }
}