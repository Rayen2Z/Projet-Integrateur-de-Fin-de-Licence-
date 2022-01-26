package com.unistra.codenames.RootServer;

import com.unistra.codenames.RootServer.DAO.Entities.FrDicEntity;
import com.unistra.codenames.RootServer.DAO.Repositories.FrDicRepository;
import com.unistra.codenames.RootServer.DAO.Repositories.UserRepository;
import com.unistra.codenames.RootServer.Server.RunServer;
import com.unistra.codenames.RootServer.Server.Service.CardManager;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.CommandLineRunner;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

import java.util.Optional;

@SpringBootApplication
public class RootServerApplication implements CommandLineRunner {
	final RunServer runServer;
/*
	@Autowired
	private CardManager cardManager;
*/
	@Autowired
	public RootServerApplication(RunServer runServer) {
		this.runServer = runServer;
	}

	@Override
	public void run(String... arg0) throws Exception {

/*
		User user = new User();
		user.setPseudo("testPlayerModified");
		user.setMdp("testMdp");
		user.setTyp('A');
		user.setNb_carte_noir(0);
		user.setNb_partie_gagnee(0);


		userRepository.save(user);
*/

/*
		Optional<FrDicEntity> res = frDicRepository.findById(2);
		System.out.println("user pseudo: " + res.get().toString());
*/
//		System.out.println(cardManager.getWordPool(1,0).toString());
		runServer.run(arg0);
	}

	public static void main(String[] args) {
		SpringApplication.run(RootServerApplication.class, args);
	}
}
