package com.unistra.codenames.RootServer.DAO.Repositories;

import com.unistra.codenames.RootServer.DAO.Entities.ThemesEntity;
import org.springframework.data.repository.CrudRepository;

public interface ThemesRepository extends CrudRepository<ThemesEntity, Integer> {
}
