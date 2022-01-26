package com.unistra.codenames.RootServer.Server.Service;

import com.unistra.codenames.RootServer.DAO.Repositories.ThemesRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.*;

@Service
public class ThemeManager {
    private static Map<String, Integer> themeMap = new TreeMap<>();
    private List<Map.Entry<String,Integer>> list;
    private static List<String> sortedThemes = new LinkedList<>();
    private ThemesRepository themesRepository;

    @Autowired
    public ThemeManager(ThemesRepository themesRepository) {
        this.themesRepository = themesRepository;
        this.themesRepository.findAll().forEach(e -> themeMap.put(e.getTheme(), 0));
        this.list = new ArrayList<>(themeMap.entrySet());
        sortByPopularity();
    }

    public void sortByPopularity() {
        this.list.clear();
        this.list.addAll(themeMap.entrySet());
        sortedThemes.clear();
        list.sort((o1, o2) -> o2.getValue().compareTo(o1.getValue()));
        list.forEach(e -> sortedThemes.add(e.getKey()));

    }

    public void addPopularity(String theme) {
        themeMap.replace(theme, themeMap.get(theme) + 1);
    }

    public List<String> getThemeList() {
        return sortedThemes;
    }
}
