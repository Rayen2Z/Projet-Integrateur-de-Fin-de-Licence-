package com.unistra.codenames.RootServer.Server.Service;

import com.unistra.codenames.RootServer.DAO.Entities.EnDicEntity;
import com.unistra.codenames.RootServer.DAO.Entities.FrDicEntity;
import com.unistra.codenames.RootServer.DAO.Entities.ThemesEntity;
import com.unistra.codenames.RootServer.DAO.Repositories.EnDicRepository;
import com.unistra.codenames.RootServer.DAO.Repositories.FrDicRepository;
import com.unistra.codenames.RootServer.DAO.Repositories.LanguageRepository;
import com.unistra.codenames.RootServer.DAO.Repositories.ThemesRepository;
import com.unistra.codenames.RootServer.DTO.Room;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.LinkedList;
import java.util.Optional;
import java.util.concurrent.ThreadLocalRandom;

@Service
public class CardManager {
    private static ArrayList<ThemesEntity> themeEntityList;
    private static ArrayList<String> themeList;
    private ThemesRepository themesRepository;
    private EnDicRepository enDicRepository;
    private FrDicRepository frDicRepository;
    private LanguageRepository languageRepository;

    @Autowired
    public CardManager(ThemesRepository themesRepository, FrDicRepository frDicRepository,
                       EnDicRepository enDicRepository, LanguageRepository languageRepository) {
        this.themesRepository = themesRepository;
        this.frDicRepository = frDicRepository;
        this.enDicRepository = enDicRepository;
        this.languageRepository = languageRepository;

        themeEntityList = new ArrayList<ThemesEntity>();
        themeList = new ArrayList<String>();
        themesRepository.findAll().forEach((e)-> {
            themeEntityList.add(e);
            themeList.add(e.getTheme());
        });
    }

    public ArrayList<String> getThemes() {
        return themeList;
    }

    public Integer getThemeID(String theme) {
        // SQL auto increments from 1
        return themeList.indexOf(theme) + 1;
    }


    public ArrayList<String> getWordPool(Integer themeID, Integer languageID) {
        ArrayList<String> wordList= new ArrayList<String>();
        if (themeID == 0 || themeID >=themeList.size()) {
            System.out.println("Illegal themeID");
            return null;
        }


        if (languageID == Room.Language.FRENCH.getValue()) {
            Optional<Iterable<FrDicEntity>> dictionary = frDicRepository.findAllById_theme(themeID);
            dictionary.ifPresent(frDicEntities -> frDicEntities.forEach((e) -> wordList.add(e.getWord())));
        } else if (languageID == Room.Language.ENGLISH.getValue()){
            Optional<Iterable<EnDicEntity>> dictionary = enDicRepository.findAllById_theme(themeID);
            dictionary.ifPresent(enDicEntities -> enDicEntities.forEach((e) -> wordList.add(e.getWord())));
        } else {
            System.out.println("Illegal languageID");
            return null;
        }

        if (wordList.size() > 0)
            return wordList;

        System.out.println("JPA error");
        return null;
    }

    public ArrayList<String> getRandomCards(Integer themeID, Integer languageID, Integer quantity) {
        LinkedList<String> wordPool = new LinkedList<String>(getWordPool(themeID, languageID));
        int wordPoolSize = wordPool.size();
        if (wordPool == null) {
            return null;
        }
        if (wordPoolSize / 2 < quantity) {
            while (wordPoolSize > quantity) {
                wordPool.remove(ThreadLocalRandom.current().nextInt(0, wordPoolSize));
                wordPoolSize--;
            }
            return new ArrayList<String>(wordPool);
        } else {
            ArrayList<String> result = new ArrayList<String>();
            while (quantity > 0) {
                result.add(wordPool.remove(ThreadLocalRandom.current().nextInt(0, wordPoolSize)));
                quantity--;
                //wordPoolSize--;
            }
            return result;
        }
    }

}
