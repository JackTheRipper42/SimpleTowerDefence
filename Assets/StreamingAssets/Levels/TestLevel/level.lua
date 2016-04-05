function getCash(wave)
	return 5000;
end

function setupWave(wave)
	setupPath1(wave)
end

function setupPath1(wave)
	assert(path1Spawner~=nil, "'path1Spawner' does not exist.");
	print(string.format("wave %i", wave));
	
	hpBoost = math.pow(1.05, wave);
	print(string.format("hpBoost %f", hpBoost));
	
	for i=1,10 do 
		for i=1,5 do
			path1Spawner.Spawn(
			{
				id = 'SmallWalker',
				healthBoost = hpBoost
			});
			path1Spawner.Wait(0.25);
		end		
		path1Spawner.Wait(1.2);
		path1Spawner.Spawn({id = 'BigWalker'});
		path1Spawner.Wait(0.3);
	end
end