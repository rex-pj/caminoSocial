import React from "react";
import Article from "../../components/templates/Article";
import { UrlConstant } from "../../utils/Constants";
import { withRouter } from "react-router-dom";
import { useQuery } from "@apollo/client";
import { GET_ARTICLES } from "../../utils/GraphQLQueries/queries";
import Loading from "../../components/atoms/Loading";
import ErrorBlock from "../../components/atoms/ErrorBlock";

export default withRouter(function (props) {
  const { match } = props;
  const { params } = match;
  const { pageNumber } = params;
  const { loading, data, error } = useQuery(GET_ARTICLES, {
    variables: {
      criterias: {
        page: pageNumber ? parseInt(pageNumber) : 1,
      },
    },
  });

  if (loading || !data) {
    return <Loading>Loading</Loading>;
  } else if (error) {
    return <ErrorBlock>Error!</ErrorBlock>;
  }

  const { articles: articlesResponse } = data;
  const { collections } = articlesResponse;
  const articles = collections.map((item) => {
    let article = { ...item };
    article.url = `${UrlConstant.Article.url}${article.id}`;
    if (article.thumbnailId) {
      article.thumbnailUrl = `${process.env.REACT_APP_CDN_PHOTO_URL}${article.thumbnailId}`;
    }

    article.creator = {
      createdDate: item.createdDate,
      profileUrl: `/profile/${item.createdByIdentityId}`,
      name: item.createdBy,
    };

    if (item.createdByPhotoCode) {
      article.creator.photoUrl = `${process.env.REACT_APP_CDN_AVATAR_API_URL}${item.createdByPhotoCode}`;
    }

    return article;
  });

  const baseUrl = "/articles";
  const { totalPage, filter } = articlesResponse;
  const { page } = filter;

  const breadcrumbs = [
    {
      isActived: true,
      title: "Article",
    },
  ];

  return (
    <Article
      articles={articles}
      breadcrumbs={breadcrumbs}
      totalPage={totalPage}
      baseUrl={baseUrl}
      currentPage={page}
    />
  );
});
