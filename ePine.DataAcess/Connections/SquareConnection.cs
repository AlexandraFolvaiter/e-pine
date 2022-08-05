using Square;

namespace ePine.DataAccess.Connections;

public class SquareConnection
{
    public SquareClient GetSquareClient(string accessToken)
    {
        return new SquareClient.Builder()
            .Environment(Square.Environment.Sandbox)
            .AccessToken(accessToken)
            .Build();
    }

}