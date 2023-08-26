namespace Component;

/// <summary>
/// Accleration Parameter Resource.
/// Useful for uniformly accelerated linear motion.
/// </summary>
public partial class AccelerationParam
{
    // the default argument is used by MF general enemies 

    public float Acceleration { get; set; } = 1000f;
    public float Deceleration { get; set; } = 1000f;
    public float MaxSpeed { get; set; } = 500f;

    public AccelerationParam() { }
    public AccelerationParam(float acc, float dec, float maxSpeed)
    {
        Acceleration = acc;
        Deceleration = dec;
        MaxSpeed = maxSpeed;
    }

    /// <summary>
    /// Process speed with param and return a new speed.
    /// </summary>
    public float ProcessSpeed(float speed, double delta)
    {
        if (speed < MaxSpeed) 
        {
            speed += (float)(Acceleration * delta);
            if (speed >= MaxSpeed) { return MaxSpeed; }
        }

        if (speed > MaxSpeed) 
        {
            speed -= (float)(Deceleration * delta);
            if (speed <= MaxSpeed) { return MaxSpeed; }
        }

        return speed;
    }

    /// <summary>
    /// Process with SpeedParam.
    /// Be care of changing <c>SpeedParam.Direction</c> unexpectedly.
    /// </summary>
    public void ProcessSpeed(SpeedParam param, double delta)
    {
        param.Speed = ProcessSpeed(param.Speed, delta);
    }
}