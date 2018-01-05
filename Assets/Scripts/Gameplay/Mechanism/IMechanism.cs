internal interface IMechanism
{
	bool Activate(int locationState = 1, int rotationState = 1, int scaleState = 1, bool relative = true);
}