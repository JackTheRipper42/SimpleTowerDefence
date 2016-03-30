function setupWave(wave)
	assert(Path1Spawner~=nil, "Path1Spawner does not exist.");

	debug.Log(string.format("wave %i", wave));
	Path1Spawner.Wait(4);
	for i=1,10 do 
		for i=1,5 do
			Path1Spawner.SpawnSmallWalker();
			Path1Spawner.Wait(0.25);
		end		
		Path1Spawner.Wait(1.2);
		Path1Spawner.SpawnBigWalker();
		Path1Spawner.Wait(0.3);
	end
end